using Eagle.Domain;
using Elk.EntityFrameworkCore;

namespace Eagle.EFDataAccess
{
    public class UserInRoleRepo : EfGenericRepo<UserInRole>
    {
        public UserInRoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
