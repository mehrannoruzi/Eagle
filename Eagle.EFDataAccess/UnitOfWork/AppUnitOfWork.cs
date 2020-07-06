using System;
using System.Threading;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Eagle.EFDataAccess
{
    public sealed class AppUnitOfWork : IElkUnitOfWork
    {
        private readonly AppDbContext _efDbContext;
        private readonly IServiceProvider _serviceProvider;

        public AppUnitOfWork(AppDbContext efDbContext,
            IServiceProvider serviceProvider)
        {
            _efDbContext = efDbContext;
            _serviceProvider = serviceProvider;
        }


        //public IGenericRepo<Person> PersonRepo => _serviceProvider.GetService<IGenericRepo<Person>>();


        public ChangeTracker ChangeTracker { get => _efDbContext.ChangeTracker; }
        public DatabaseFacade Database { get => _efDbContext.Database; }

        public SaveChangeResult ElkSaveChanges()
            => _efDbContext.ElkSaveChanges();

        public Task<SaveChangeResult> ElkSaveChangesAsync(CancellationToken cancellationToken = default)
            => _efDbContext.ElkSaveChangesAsync(cancellationToken);

        public SaveChangeResult ElkSaveChangesWithValidation()
            => _efDbContext.ElkSaveChangesWithValidation();

        public Task<SaveChangeResult> ElkSaveChangesWithValidationAsync(CancellationToken cancellationToken = default)
            => _efDbContext.ElkSaveChangesWithValidationAsync(cancellationToken);

        public int SaveChanges()
            => _efDbContext.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _efDbContext.SaveChangesAsync(cancellationToken);

        public Task<int> SaveChangesAsync(bool AcceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => _efDbContext.SaveChangesAsync(AcceptAllChangesOnSuccess, cancellationToken);

        public void Dispose() => _efDbContext.Dispose();
    }
}
