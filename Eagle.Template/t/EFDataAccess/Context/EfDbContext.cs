using Elk.Core;
using $ext_safeprojectname$.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace $ext_safeprojectname$.EFDataAccess
{
    public class EfDbContext : ElkDbContext
    {
        public EfDbContext() { }

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<User>().HasIndex(x => x.Email).HasName("IX_Email").IsUnique();
            //builder.Entity<MenuSPModel>().ToTable(nameof(MenuSPModel), schema: "Auth").HasNoKey();

            builder.OverrideDeleteBehavior();
            builder.RegisterAllEntities<IEntity>(typeof(Person).Assembly);
        }
    }
}
