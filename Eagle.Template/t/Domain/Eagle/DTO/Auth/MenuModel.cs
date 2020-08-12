using Elk.Core;
using System.Collections.Generic;
using System.Linq;

namespace $ext_safeprojectname$.Domain
{
    public class MenuModel
    {
        public string Menu { get; set; }
        public UserAction DefaultUserAction { get; set; }
        public IEnumerable<UserAction> ActionList { get; set; }
        public Dictionary<int, string> Roles { get => ActionList.Select(x => new { x.RoleId, x.RoleNameFa }).Distinct().ToDictionary(x => x.RoleId, x => x.RoleNameFa); }
    }
}
