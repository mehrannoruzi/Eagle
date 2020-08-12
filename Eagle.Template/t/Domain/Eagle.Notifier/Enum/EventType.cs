using System.ComponentModel;

namespace $ext_safeprojectname$.Domain
{
    public enum EventType : byte
    {
        [Description("ثبت نام")]
        Subscription = 1,

        [Description("کراول")]
        Crawl = 2,

        [Description("سفارش")]
        Order = 3
    }
}
