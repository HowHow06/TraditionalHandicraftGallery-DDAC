using System;
using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.Models
{
    public class QuoteRequest : IHasTimeStamp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserID { get; set; } // Foreign Key

        [Required]
        public int HandicraftID { get; set; } // Foreign Key

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // Nullable DateTime

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual Handicraft Handicraft { get; set; }
    }
}
