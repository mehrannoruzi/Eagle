using Eagle.Domain;
using Elk.EntityFrameworkCore;

namespace Eagle.DataAccess.Ef
{
    public class RoleRepo : EfGenericRepo<Role>
    {
        public RoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
