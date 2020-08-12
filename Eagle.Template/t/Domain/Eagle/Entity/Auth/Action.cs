using Elk.Core;
using $ext_safeprojectname$.Domain.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $ext_safeprojectname$.Domain
{

    [Table(nameof(Action), Schema = "Auth")]
    public class Action : IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }

        [Display(Name = nameof(Strings.Parent), ResourceType = typeof(Strings))]
        [ForeignKey(nameof(ParentId))]
        public Action Parent { get; set; }

        [Display(Name = nameof(Strings.Parent), ResourceType = typeof(Strings))]
        public int? ParentId { get; set; }

        [Display(Name = nameof(Strings.OrderPriority), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        public byte OrderPriority { get; set; }

        [Display(Name = nameof(Strings.ShowInMenu), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        public bool ShowInMenu { get; set; }

        [Column(TypeName = "varchar(25)")]
        [Display(Name = nameof(Strings.ControllerName), ResourceType = typeof(Strings))]
        [MaxLength(25, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(25, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string ControllerName { get; set; }

        [Column(TypeName = "varchar(25)")]
        [Display(Name = nameof(Strings.ActionName), ResourceType = typeof(Strings))]
        [MaxLength(25, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(25, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string ActionName { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        [MaxLength(55, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(55, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        [Display(Name = nameof(Strings.Icon), ResourceType = typeof(Strings))]
        [MaxLength(40, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(40, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        public string Icon { get; set; }

        [NotMapped]
        [Display(Name = nameof(Strings.Path), ResourceType = typeof(Strings))]
        public string Path { get { return $"/{ControllerName}/{ActionName}"; } }

        public virtual ICollection<ActionInRole> ActionInRoles { get; set; }
    }
}
