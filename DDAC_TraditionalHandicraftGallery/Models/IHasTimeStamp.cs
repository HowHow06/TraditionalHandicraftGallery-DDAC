using System;
using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.Models
{
    public interface IHasTimeStamp
    {
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } // Nullable DateTime
    }
}
