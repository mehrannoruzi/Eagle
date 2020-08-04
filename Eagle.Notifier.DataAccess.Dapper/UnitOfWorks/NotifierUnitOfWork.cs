using System;
using Eagle.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Eagle.Notifier.DataAccess.Dapper
{
    public sealed class NotifierUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        public NotifierUnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public IEventMapperRepo EventMapperRepo => _serviceProvider.GetService<IEventMapperRepo>();
        public INotificationRepo NotificationRepo => _serviceProvider.GetService<INotificationRepo>();
    }
}