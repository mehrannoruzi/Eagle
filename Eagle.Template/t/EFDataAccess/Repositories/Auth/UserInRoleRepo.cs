using $ext_safeprojectname$.Domain;
using Elk.EntityFrameworkCore;

namespace $ext_safeprojectname$.EFDataAccess
{
    public class UserInRoleRepo : EfGenericRepo<UserInRole>
    {
        public UserInRoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
