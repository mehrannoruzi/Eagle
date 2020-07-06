using Elk.EntityFrameworkCore;

namespace Eagle.EFDataAccess
{
    public class AppGenericRepo<T> : EfGenericRepo<T> where T : class
    {
        public AppGenericRepo(AppDbContext appDbContext) : base(appDbContext) { }
    }
}