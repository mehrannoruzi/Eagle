using System;

namespace Eagle.Constant
{
    public static class CacheSettings
    {
        public static string GetMenuModelKey(Guid userId) => userId.ToString().Replace("-", "_");
    }
}
