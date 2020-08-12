using $ext_safeprojectname$.Domain;
using Elk.EntityFrameworkCore;

namespace $ext_safeprojectname$.DataAccess.Ef
{
    public class RoleRepo : EfGenericRepo<Role>
    {
        public RoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
