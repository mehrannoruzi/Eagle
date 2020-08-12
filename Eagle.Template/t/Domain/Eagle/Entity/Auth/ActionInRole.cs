using Elk.Core;
using $ext_safeprojectname$.Domain.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $ext_safeprojectname$.Domain
{
    [Table(nameof(ActionInRole), Schema = "Auth")]
    public class ActionInRole : IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionInRoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.Role))]
        public int RoleId { get; set; }

        [ForeignKey(nameof(ActionId))]
        public Action Action { get; set; }

        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.Action))]
        public int ActionId { get; set; }

        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.IsDefault))]
        public bool IsDefault { get; set; }

    }
}
