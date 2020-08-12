using System.ComponentModel;

namespace $ext_safeprojectname$.Domain
{
    public enum NotificationType : byte
    {
        [Description("پیامک")]
        Sms = 1,

        [Description("تلگرام")]
        TeleBot = 2,

        [Description("ایمیل")]
        Email = 3
    }
}
