using System;

namespace $ext_safeprojectname$.InfraStructure
{
    public static class GlobalVariables
    {
        public static class CacheSettings
        {
            public static string GetMenuModelKey(Guid userId) => userId.ToString().Replace("-", "_");
        }

        public static class SmsProviders
        {
            public static class LinePayamak
            {
                public static string Username = "500096998998";
                public static string Password = "80225353";
                public static string SenderId = "500096998998";
            }
        }
    }
}