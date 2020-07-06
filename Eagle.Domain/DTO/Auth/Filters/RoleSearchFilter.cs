using Elk.Core;
using Eagle.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace Eagle.Domain
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
