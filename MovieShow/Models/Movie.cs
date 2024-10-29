using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShow.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Automatically generate this value
        public int MovieId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public Genres Genre { get; set; }

        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Cast { get; set; }
        public string TrailerUrl { get; set; }
        public string VideoUrl { get; set; }
        public decimal Rating { get; set; }

        public string? Img { get; set; }


        public enum Genres
        {
            Action,
            Comedy,
            Drama,
            Horror,
            SciFi,
            Documentary
        }

        public bool IsDeleted { get; set; }
        public int Seasons {  get; set; }
    }
}
