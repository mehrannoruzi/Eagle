using System;
using Eagle.Domain;
using System.Threading.Tasks;

namespace Eagle.Notifier.Service
{
    public class SendEmailStrategy : ISendStrategy
    {
        public Task SendAsync(Notification notification, INotificationRepo notifierUnitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}
