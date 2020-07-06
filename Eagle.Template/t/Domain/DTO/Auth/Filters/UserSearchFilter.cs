using Elk.Core;
using $ext_safeprojectname$.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace $ext_safeprojectname$.Domain
{
    public class UserSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        public string MobileNumberF { get; set; }

        [Display(Name = nameof(Strings.FullName), ResourceType = typeof(Strings))]
        [MaxLength(60, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(60, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string FullNameF { get; set; }

        [Display(Name = nameof(Strings.Email), ResourceType = typeof(Strings))]
        [MaxLength(50, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(50, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string EmailF { get; set; }
    }
}
