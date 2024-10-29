using Microsoft.EntityFrameworkCore;

namespace MovieShow.Models.DataModels
{
    public class IndexDataModel
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Combine> Combines { get; set; }



    }
}
