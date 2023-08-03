using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDAC_TraditionalHandicraftGallery.Models
{
    public class HandicraftType : IHasTimeStamp
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } // Nullable DateTime

        // Navigation property
        public virtual ICollection<Handicraft> Handicrafts { get; set; }
    }
}
