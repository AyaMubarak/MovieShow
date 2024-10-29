using System.Collections.Generic;

namespace MovieShow.Models
{
    public class Season
    {
        public int SeasonId { get; set; }
        public int SeasonNumber { get; set; }
        public int SerieId { get; set; }

        public Serie Serie { get; set; }
        public List<Episode> Episode { get; set; } = new List<Episode>(); // تأكد من تهيئتها
    }
}