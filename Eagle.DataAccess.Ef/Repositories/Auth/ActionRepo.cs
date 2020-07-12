using Eagle.Domain;
using Elk.EntityFrameworkCore;

namespace Eagle.DataAccess.Ef
{
    public class ActionRepo : EfGenericRepo<Action>
    {
        public ActionRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
