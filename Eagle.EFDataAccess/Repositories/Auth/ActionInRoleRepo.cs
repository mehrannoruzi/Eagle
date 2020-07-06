using Eagle.Domain;
using Elk.EntityFrameworkCore;

namespace Eagle.EFDataAccess
{
    public class ActionInRoleRepo : EfGenericRepo<ActionInRole>
    {
        public ActionInRoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
