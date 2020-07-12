using System;

namespace Eagle.InfraStructure
{
    public static class GlobalVariables
    {
        public static class CacheSettings
        {
            public static string GetMenuModelKey(Guid userId) => userId.ToString().Replace("-", "_");
        }
    }
}