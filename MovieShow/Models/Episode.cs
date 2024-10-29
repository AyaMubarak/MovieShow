namespace MovieShow.Models
{
    public class Episode
    {
        public int EpisodeId { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonId { get; set; }
        public string VideoUrl { get; set; }

        public Season Season { get; set; } // تصحيح اسم العلاقة
    }
}
