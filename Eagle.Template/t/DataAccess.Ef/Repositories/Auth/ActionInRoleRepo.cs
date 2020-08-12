using $ext_safeprojectname$.Domain;
using Elk.EntityFrameworkCore;

namespace $ext_safeprojectname$.DataAccess.Ef
{
    public class ActionInRoleRepo : EfGenericRepo<ActionInRole>
    {
        public ActionInRoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
