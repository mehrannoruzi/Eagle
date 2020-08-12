using System;
using $ext_safeprojectname$.Domain;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using System.Data.SqlClient;

namespace $ext_safeprojectname$.DataAccess.Ef
{
    public class UserRepo : EfGenericRepo<User>, IUserRepo
    {
        private readonly AuthDbContext _db;

        public UserRepo(AuthDbContext authContext) : base(authContext)
        {
            _db = authContext;
        }

        public async Task<List<MenuSPModel>> GetUserMenu(Guid userId) =>
                 await _db.MenuSPModel.FromSqlRaw("[Auth].[GetUserMenu] @UserId = {0}", userId).ToListAsync();

    }
}