using MovieShow;
using Microsoft.AspNetCore.Mvc;
using MovieShow.Data;

namespace MovieShow.ViewComponents
{
    public class MovieViewComponent : ViewComponent
    {
        private AppDbContext db;
        public MovieViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public IViewComponentResult Invoke()
        {
            return View(db.Movies.OrderByDescending(x => x.MovieId).ToList());
        }
    }
}