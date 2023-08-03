using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.Models
{
    public class ApplicationUser : IdentityUser, IHasTimeStamp
    {
        [Required]
        public bool IsAdmin { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // Nullable DateTime

        // Navigation property
        public virtual ICollection<QuoteRequest> QuoteRequests { get; set; }
    }
}