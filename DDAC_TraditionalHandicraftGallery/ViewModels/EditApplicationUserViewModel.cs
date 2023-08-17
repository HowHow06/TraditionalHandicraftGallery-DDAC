using DDAC_TraditionalHandicraftGallery.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.ViewModels
{
    public class EditApplicationUserViewModel : ApplicationUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }

        [Required]
        public override string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }  

        [DataType(DataType.Password)]
        [Display(Name = "Re-type Password")]
        public string ConfirmPassword { get; set; }
    }
}
