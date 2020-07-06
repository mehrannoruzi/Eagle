using System;
using $ext_safeprojectname$.Domain;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace $ext_safeprojectname$.EFDataAccess
{
    public class UserRepo : EfGenericRepo<User>, IUserRepo
    {
        private readonly AuthDbContext _authContext;

        public UserRepo(AuthDbContext authContext) : base(authContext)
        {
            _authContext = authContext;
        }

        public async Task<List<MenuSPModel>> GetUserMenu(Guid userId)
            => await _authContext.Set<MenuSPModel>().FromSqlRaw("EXEC [Auth].[GetUserMenu] {0}", userId).ToListAsync();

        public async Task<User> FindByUsername(string username) => await FirstOrDefaultAsync(x => x.Email == username);
    }
}
