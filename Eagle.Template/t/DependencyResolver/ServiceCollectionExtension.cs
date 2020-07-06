using Elk.Core;
using Elk.Cache;
using $ext_safeprojectname$.Domain;
using $ext_safeprojectname$.Service;
using $ext_safeprojectname$.EFDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace $ext_safeprojectname$.DependencyResolver
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTransient(this IServiceCollection serviceCollection, IConfiguration _configuration)
        {
            //serviceCollection.AddTransient<IUserRepo, UserRepo>();

            return serviceCollection;
        }

        public static IServiceCollection AddScoped(this IServiceCollection serviceCollection, IConfiguration _configuration)
        {
            serviceCollection.AddScoped<AuthUnitOfWork, AuthUnitOfWork>();
            serviceCollection.AddScoped<EfUnitOfWork, EfUnitOfWork>();

            serviceCollection.AddScoped<IMemoryCacheProvider, MemoryCacheProvider>();

            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IRoleService,RoleService>();
            serviceCollection.AddScoped<IActionService,ActionService>();
            serviceCollection.AddScoped<IActionInRoleService,ActionInRoleService>();
            serviceCollection.AddScoped<IUserInRoleService,UserInRoleService>();
            serviceCollection.AddScoped<IUserActionProvider, UserService>();

            serviceCollection.AddScoped<IUserRepo, UserRepo>();
            serviceCollection.AddScoped<IGenericRepo<Role>, RoleRepo>();
            serviceCollection.AddScoped<IGenericRepo<Action>, ActionRepo>();
            serviceCollection.AddScoped<IGenericRepo<ActionInRole>, ActionInRoleRepo>();
            serviceCollection.AddScoped<IGenericRepo<UserInRole>, UserInRoleRepo>();

            return serviceCollection;
        }

        public static IServiceCollection AddSingleton(this IServiceCollection services, IConfiguration _configuration)
        {
            services.AddSingleton<IEmailService>(s => new EmailService(
                _configuration["CustomSettings:EmailServiceConfig:EmailHost"],
                _configuration["CustomSettings:EmailServiceConfig:EmailUserName"],
                _configuration["CustomSettings:EmailServiceConfig:EmailPassword"]));
            return services;
        }

        public static IServiceCollection AddContext<TDbContext>(this IServiceCollection serviceCollection, string conectionString) where TDbContext : DbContext
        {
            serviceCollection.AddDbContext<TDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(conectionString,
                            sqlServerOption =>
                            {
                                sqlServerOption.MaxBatchSize(1000);
                                sqlServerOption.CommandTimeout(null);
                                sqlServerOption.UseRelationalNulls(false);
                                //sqlServerOption.EnableRetryOnFailure();
                                //sqlServerOption.UseRowNumberForPaging(false);
                            });
                //.AddInterceptors(new DbContextInterceptors());
            });

            //serviceCollection.AddDbContextPool<TDbContext>(optionBuilder =>
            //{
            //    optionBuilder.UseSqlServer(conectionString,
            //                sqlServerOption =>
            //                {
            //                    sqlServerOption.MaxBatchSize(1000);
            //                    sqlServerOption.CommandTimeout(null);
            //                    //sqlServerOption.EnableRetryOnFailure();
            //                    sqlServerOption.UseRelationalNulls(false);
            //                    //sqlServerOption.UseRowNumberForPaging(false);
            //                });
            //    //.AddInterceptors(new DbContextInterceptors());
            //});

            return serviceCollection;
        }
    }
}
