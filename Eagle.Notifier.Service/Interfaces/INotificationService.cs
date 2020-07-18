using Elk.Core;
using Eagle.Domain;
using System.Threading.Tasks;

namespace Eagle.Notifier.Service
{
    public interface INotificationService
    {
        Task<IResponse<bool>> AddAsync(NotificationDto notifyDto, int applicationId);
        
        Task SendAsync();
    }
}
