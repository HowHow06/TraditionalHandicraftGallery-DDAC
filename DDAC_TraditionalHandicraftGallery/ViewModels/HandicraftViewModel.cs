using DDAC_TraditionalHandicraftGallery.Models;
using Microsoft.AspNetCore.Http;

namespace DDAC_TraditionalHandicraftGallery.ViewModels
{
    public class HandicraftViewModel : Handicraft
    {
        public new string ImageURL { get; set; } = "";
        public IFormFile ImageURLFile { get; set; }
    }
}
