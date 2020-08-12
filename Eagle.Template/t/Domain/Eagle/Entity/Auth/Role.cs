using Elk.Core;
using $ext_safeprojectname$.Domain.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $ext_safeprojectname$.Domain
{
    [Table(nameof(Role), Schema = "Auth")]
    public class Role : IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Display(Name = nameof(Strings.Enabled), ResourceType = typeof(Strings))]
        public bool Enabled { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = nameof(Strings.NameFa), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        [MaxLength(30, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(30, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string RoleNameFa { get; set; }

        [Column(TypeName = "varchar(30)")]
        [Display(Name = nameof(Strings.NameEn), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        [MaxLength(30, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(30, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string RoleNameEn { get; set; }

        public ICollection<ActionInRole> ActionInRoles { get; set; }
        public ICollection<UserInRole> UserInRoles { get; set; }
    }

}
