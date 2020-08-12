using System;
using Elk.Core;
using $ext_safeprojectname$.Domain.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $ext_safeprojectname$.Domain
{
    [Table(nameof(UserInRole), Schema = "Auth")]
    public class UserInRole : IAuthEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserInRoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        [Display(Name = nameof(Strings.Role), ResourceType = typeof(Strings))]
        public int RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.User))]
        public User User { get; set; }

        [Display(Name = nameof(Strings.Username),ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings))]
        public Guid UserId { get; set; }
    }
}
