using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.ViewModels
{
    public class EditPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Re-type Password")]
        public string ConfirmPassword { get; set; }
    }
}
