using System;
using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.Models
{
    public class Handicraft : IHasTimeStamp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string AuthorName { get; set; }

        [Required]
        [EmailAddress]
        public string AuthorEmail { get; set; }

        [Required]
        public int TypeId { get; set; } // Foreign Key

        [Required]
        public bool IsHidden { get; set; } = false;

        [Required]
        public string ImageURL { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // Nullable DateTime

        // Navigation property
        public virtual HandicraftType Type { get; set; }

    }
}
