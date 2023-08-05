using DDAC_TraditionalHandicraftGallery.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.ViewModels
{
    public class ApplicationUserViewModel : ApplicationUser
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }  
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
