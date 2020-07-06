using System;
using Elk.Core;
using Elk.Cache;
using System.Text;
using System.Linq;
using $ext_safeprojectname$.Domain;
using $ext_safeprojectname$.EFDataAccess;
using System.Threading.Tasks;
using System.Linq.Expressions;
using $ext_safeprojectname$.Service.Resourses;
using System.Collections.Generic;
using DomainStrings = $ext_safeprojectname$.Domain.Resources.Strings;

namespace $ext_safeprojectname$.Service
{
    public class UserService : IUserService, IUserActionProvider
    {
        private readonly AuthUnitOfWork _uow;
        private readonly IEmailService _emailService;
        private readonly IMemoryCacheProvider _cache;

        public UserService(AuthUnitOfWork uow, IMemoryCacheProvider cache, IEmailService emailService)
        {
            _uow = uow;
            _cache = cache;
            _emailService = emailService;
        }


        #region CRUD
        public async Task<IResponse<User>> AddAsync(User model)
        {
            model.Password = HashGenerator.Hash(model.Password);
            await _uow.UserRepo.AddAsync(model);

            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<User> { Result = model, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<User>> UpdateProfile(User model)
        {
            var findedUser = await _uow.UserRepo.FindAsync(model.UserId);
            if (findedUser == null) return new Response<User> { Message = Strings.RecordNotExist.Fill(DomainStrings.User) };

            findedUser.Password = HashGenerator.Hash(model.NewPassword);
            findedUser.FullName = model.FullName;

            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<User> { Result = findedUser, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<User>> UpdateAsync(User model)
        {
            var findedUser = await _uow.UserRepo.FindAsync(model.UserId);
            if (findedUser == null) return new Response<User> { Message = Strings.RecordNotExist.Fill(DomainStrings.User) };

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
                findedUser.Password = HashGenerator.Hash(model.NewPassword);

            findedUser.FullName = model.FullName;
            findedUser.Enabled = model.Enabled;

            var saveResult = _uow.ElkSaveChangesAsync();
            return new Response<User> { Result = findedUser, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(Guid userId)
        {
            _uow.UserRepo.Delete(new User { UserId = userId });
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<User>> FindAsync(Guid userId)
        {
            var findedUser = await _uow.UserRepo.FindAsync(userId);
            if (findedUser == null) return new Response<User> { Message = Strings.RecordNotExist.Fill(DomainStrings.User) };

            return new Response<User> { Result = findedUser, IsSuccessful = true };
        }
        #endregion


        private string MenuModelCacheKey(Guid userId) => $"MenuModel_{userId.ToString().Replace("-", "_")}";

        public IEnumerable<UserAction> GetUserActions(string userId, string urlPrefix = "")
            => GetAvailableActions(Guid.Parse(userId), null, urlPrefix).Result.ActionList;

        public async Task<IEnumerable<UserAction>> GetUserActionsAsync(string userId, string urlPrefix = "")
            => (await GetAvailableActions(Guid.Parse(userId), null, urlPrefix)).ActionList;

        public async Task<IResponse<User>> Authenticate(string username, string password)
        {
            var user = await _uow.UserRepo.FindByUsername(username);
            if (user == null) return new Response<User> { Message = Strings.InvalidUsernameOrPassword };
            
            if (!user.Enabled) return new Response<User> { Message = Strings.AccountIsBlocked };
            
            var hashedPassword = HashGenerator.Hash(password);
            if (user.Password != hashedPassword && user.NewPassword != hashedPassword)
            {
                FileLoger.Message($"UserService/Authenticate-> Invalid Password Login ! Username:{username} Password:{password}");
                return new Response<User> { Message = Strings.InvalidUsernameOrPassword };
            }
            if (user.NewPassword == hashedPassword)
            {
                user.Password = user.NewPassword;
                user.NewPassword = null;
            }
            user.LastLoginDateMi = DateTime.Now;
            user.LastLoginDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);

            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<User> { IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message, Result = user };
        }

        private string GetAvailableMenu(List<MenuSPModel> spResult, string urlPrefix = "")
        {
            var sb = new StringBuilder(string.Empty);
            foreach (var item in spResult.Where(x => x.ShowInMenu && (x.IsAction || (!x.IsAction && x.ActionsList.Any(v => v.ShowInMenu)))).OrderBy(x => x.OrderPriority))
            {
                if (!item.IsAction && !item.HasChild) continue;
                #region Add Menu
                sb.AppendFormat("<li {0}><a href='{1}'><i class='{2} default-i'></i><span class='nav-label'>{3}</span> {4}</a>",
                            item.IsAction ? "" : "class='link-parent'",
                            item.IsAction ? $"{urlPrefix}/{item.ControllerName.ToLower()}/{item.ActionName.ToLower()}" : "#",
                            item.Icon,
                            item.Name,
                            item.IsAction ? "" : "<span class='fa arrow'></span>"); 
                #endregion

                if (!item.IsAction && item.HasChild)
                {
                    #region Add Sub Menu
                    sb.Append("<ul class='nav nav-second-level collapse'>");
                    foreach (var v in item.ActionsList.Where(x => x.ShowInMenu).OrderBy(x => x.OrderPriority))
                    {
                        sb.AppendFormat("<li><a href='{0}' >{1}</a><li>",
                        $"{urlPrefix}/{v.ControllerName.ToLower()}/{v.ActionName.ToLower()}",
                        v.Name);
                    }
                    sb.Append("</ul>"); 
                    #endregion
                }
                sb.Append("</li>");
            }
            return sb.ToString();
        }

        public async Task<MenuModel> GetAvailableActions(Guid userId, List<MenuSPModel> spResult = null, string urlPrefix = "")
        {
            var userMenu = (MenuModel)_cache.Get(MenuModelCacheKey(userId));
            if (userMenu != null) return userMenu;

            userMenu = new MenuModel();
            if (spResult == null) spResult = await _uow.UserRepo.GetUserMenu(userId);

            #region Find Default View
            foreach (var menuItem in spResult)
            {
                var actions = menuItem.ActionsList;
                if (actions.Any(x => x.IsDefault))
                {
                    userMenu.DefaultUserAction = new UserAction
                    {
                        Action = actions.FirstOrDefault(x => x.IsDefault).ActionName,
                        Controller = actions.FirstOrDefault(x => x.IsDefault).ControllerName
                    };
                    break;
                }
            }
            if (userMenu.DefaultUserAction == null || userMenu.DefaultUserAction.Controller == null) return null;
            #endregion

            var userActions = 
                spResult.Where(x => x.IsAction)
                .Select(rvm => new UserAction
                {
                    Controller = rvm.ControllerName.ToLower(),
                    Action = rvm.ActionName.ToLower(),
                    RoleId = rvm.RoleId,
                    RoleNameFa = rvm.RoleNameFa
                })
             .Union(
                 spResult.Where(x => !x.IsAction)
                 .SelectMany(x => x.ActionsList.Select(rvm => new UserAction
                 {
                     Controller = rvm.ControllerName.ToLower(),
                     Action = rvm.ActionName.ToLower(),
                     RoleId = rvm.RoleId,
                     RoleNameFa = rvm.RoleNameFa
                 }))).ToList();
            userMenu.Menu = GetAvailableMenu(spResult, urlPrefix);
            userMenu.ActionList = userActions;
            
            _cache.Add(MenuModelCacheKey(userId), userMenu, DateTime.Now.AddMinutes(30));
            return userMenu;
        }

        public void SignOut(Guid userId)
        {
            _cache.Remove(MenuModelCacheKey(userId));
        }

        public PagingListDetails<User> Get(UserSearchFilter filter)
        {
            Expression<Func<User, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FullNameF))
                    conditions = x => x.FullName.Contains(filter.FullNameF);
                if (!string.IsNullOrWhiteSpace(filter.EmailF))
                    conditions = x => x.Email.Contains(filter.EmailF);
                if (!string.IsNullOrWhiteSpace(filter.MobileNumberF))
                    conditions = x => x.MobileNumber.ToString().Contains(filter.MobileNumberF);
            }
            
            var items = _uow.UserRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.InsertDateMi));
            return items;
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10) 
            => _uow.UserRepo.Get(x => x.FullName.Contains(searchParameter))
                 .Union(
                    _uow.UserRepo.Get(x => x.Email.Contains(searchParameter))).Select(x => new
                     {
                         x.UserId,
                         x.Email,
                         x.FullName
                     })
                .OrderBy(x => x.FullName)
                .Take(take)
                .ToDictionary(k => (object)k.UserId, v => (object)$"{v.FullName}({v.Email})");

        public async Task<IResponse<string>> RecoverPassword(string username, string from, EmailMessage model)
        {
            var user = await _uow.UserRepo.FindByUsername(username);
            if (user == null) return new Response<string> { Message = Strings.RecordNotExist.Fill(DomainStrings.User) };

            var newPassword = Randomizer.GetUniqueKey(6);
            user.NewPassword = HashGenerator.Hash(newPassword);
            _uow.UserRepo.Update(user);
            var saveResult = await _uow.ElkSaveChangesAsync();
            if (!saveResult.IsSuccessful) return new Response<string> { IsSuccessful = false, Message = saveResult.Message };

            model.Subject = Strings.RecoverPassword;
            model.Body = model.Body.Fill(newPassword);
            _emailService.Send(username, new List<string> { from }, model);
            return new Response<string> { IsSuccessful = true, Message = saveResult.Message };
        }
    }
}