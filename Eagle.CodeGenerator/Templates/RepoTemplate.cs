namespace Eagle.CodeGenerator.Templates
{
    public static class RepoTemplate
    {
        public static string Rep = @"
using ##ProjectName##.Domain;
using Elk.EntityFrameworkCore;

namespace ##ProjectName##.EFDataAccess
{
    public class ##entity##Repo : EfGenericRepo<##entity##>
    {
        public RoleRepo(AppDbContext appDbContext) : base(AppDbContext)
        { }
    }
}";
        public static string IRep = @"
using Elk.Core;

namespace ##ProjectName##.Domain
{
    public interface I##entity##Repo : IGenericRepo<##entity##>, IScopedInjection
    {
    }
}";

        
    }
}
