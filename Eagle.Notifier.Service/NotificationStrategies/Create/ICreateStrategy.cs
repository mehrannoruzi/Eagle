using Eagle.Domain;
using System.Threading.Tasks;

namespace Eagle.Notifier.Service
{
    public interface ICreateStrategy
    {
        Task Create(NotificationDto notifyDto, INotificationRepo notificationRepo, int applicationId);
    }
}