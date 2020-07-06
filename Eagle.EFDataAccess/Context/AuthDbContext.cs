using Elk.Core;
using Eagle.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace Eagle.EFDataAccess
{
    public class AuthDbContext : ElkDbContext
    {
        public AuthDbContext() { }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(x => x.Email).HasName("IX_Email").IsUnique();
            builder.Entity<MenuSPModel>().HasNoKey().ToView(null);

            builder.OverrideDeleteBehavior();
            builder.RegisterAllEntities<IAuthEntity>(typeof(Role).Assembly);
        }

        public DbSet<MenuSPModel> MenuSPModel { get; set; }
    }
}
