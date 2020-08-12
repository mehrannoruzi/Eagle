using Elk.Core;
using $ext_safeprojectname$.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace $ext_safeprojectname$.Domain
{
    public class RoleSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.NameFa), ResourceType = typeof(Strings))]
        [StringLength(30, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string RoleNameFaF { get; set; }

        [Display(Name = nameof(Strings.NameEn), ResourceType = typeof(Strings))]
        [StringLength(30, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string RoleNameEnF { get; set; }
    }
}
