using $ext_safeprojectname$.Domain;
using Elk.EntityFrameworkCore;

namespace $ext_safeprojectname$.EFDataAccess
{
    public class PersonRepo : EfGenericRepo<Person>
    {
        public PersonRepo(EfDbContext efDbContext) : base(efDbContext)
        { }
    }
}
