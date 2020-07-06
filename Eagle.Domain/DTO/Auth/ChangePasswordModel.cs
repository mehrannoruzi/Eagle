using Eagle.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace Eagle.Domain
{
    public class ChangePasswordModel : SignInModel
    {
        [Display(Name = nameof(Strings.NewPassword), ResourceType = typeof(Strings))]
        [MaxLength(45, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(45, MinimumLength = 5, ErrorMessageResourceName = nameof(Strings.Min5MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings), AllowEmptyStrings = false)]
        public string NewPassword { get; set; }

        [Display(Name = nameof(Strings.ConfirmPassword), ResourceType = typeof(Strings))]
        [Compare(nameof(NewPassword), ErrorMessageResourceName = nameof(Strings.PasswordsNotMatched), ErrorMessageResourceType = typeof(Strings))]
        public string ConfirmPassword { get; set; }
    }
}

