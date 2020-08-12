using $ext_safeprojectname$.Domain;
using Elk.EntityFrameworkCore;

namespace $ext_safeprojectname$.DataAccess.Ef
{
    public class ActionRepo : EfGenericRepo<Action>
    {
        public ActionRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
