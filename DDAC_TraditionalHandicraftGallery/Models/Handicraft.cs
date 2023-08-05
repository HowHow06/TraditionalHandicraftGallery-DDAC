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
        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Author Email")]
        public string AuthorEmail { get; set; }

        [Required]
        public int TypeId { get; set; } // Foreign Key

        [Required]
        [Display(Name = "Is Hidden")]
        public bool IsHidden { get; set; } = false;

        [Required]
        [Display(Name = "Image")]
        public string ImageURL { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; } // Nullable DateTime

        // Navigation property
        public virtual HandicraftType Type { get; set; }

    }
}
