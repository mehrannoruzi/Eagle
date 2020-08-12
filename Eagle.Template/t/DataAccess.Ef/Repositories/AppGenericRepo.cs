using Elk.EntityFrameworkCore;

namespace $ext_safeprojectname$.DataAccess.Ef
{
    public class AppGenericRepo<T> : EfGenericRepo<T> where T : class
    {
        public AppGenericRepo(AppDbContext appDbContext) : base(appDbContext) { }
    }
}