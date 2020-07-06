using Eagle.Domain;
using Elk.EntityFrameworkCore;

namespace Eagle.EFDataAccess
{
    public class ActionRepo : EfGenericRepo<Action>
    {
        public ActionRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
