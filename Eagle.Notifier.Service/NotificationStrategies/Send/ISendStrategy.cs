using Eagle.Domain;
using System.Threading.Tasks;

namespace Eagle.Notifier.Service
{
    public interface ISendStrategy
    {
        Task SendAsync(Notification notification, INotificationRepo notificationRepo);
    }
}
