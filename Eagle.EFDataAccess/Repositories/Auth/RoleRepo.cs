using Eagle.Domain;
using Elk.EntityFrameworkCore;

namespace Eagle.EFDataAccess
{
    public class RoleRepo : EfGenericRepo<Role>
    {
        public RoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
