using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieShow.Models
{
    public class Serie
    {
        [Key]
        public int SerieId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }
        public Genres Genre { get; set; }

        [Required]
        [MaxLength(255)]
        public string Director { get; set; }

        public decimal Rating { get; set; }
        public string? Img { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Season> Season { get; set; } = new List<Season>(); // تأكد من تهيئتها

        public enum Genres
        {
            Action,
            Comedy,
            Drama,
            Horror,
            SciFi,
            Documentary
        }
    }
}

