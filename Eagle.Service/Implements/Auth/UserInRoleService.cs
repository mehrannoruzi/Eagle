using System;
using Elk.Core;
using System.Linq;
using Eagle.Domain;
using Eagle.DataAccess.Ef;
using System.Threading.Tasks;
using Eagle.Service.Resourses;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Eagle.Service
{
    public class UserInRoleService : IUserInRoleService
    {
        private readonly AuthUnitOfWork _uow;

        public UserInRoleService(AuthUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<IResponse<UserInRole>> Add(UserInRole model)
        {
            if (await _uow.UserInRoleRepo.AnyAsync(x => x.UserId == model.UserId && x.RoleId == model.RoleId))
                return new Response<UserInRole> { Message = ServiceStrings.DuplicateRecord, IsSuccessful = false };

            await _uow.UserInRoleRepo.AddAsync(model);
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<UserInRole>
            {
                Result = model,
                Message = saveResult.Message,
                IsSuccessful = saveResult.IsSuccessful
            };
        }

        public async Task<IResponse<bool>> Delete(int userInRoleId)
        {
            _uow.UserInRoleRepo.Delete(new UserInRole { UserInRoleId = userInRoleId });
            var saveResult = await _uow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public IEnumerable<UserInRole> Get(Guid userId) 
            => _uow.UserInRoleRepo.Get(x => x.UserId == userId,
            x => x.OrderByDescending(uir => uir.UserId),
            new List<Expression<Func<UserInRole, object>>> { x => x.Role }).ToList();

    }
}
