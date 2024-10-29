using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShow;
using MovieShow.Data;

namespace MovieShow.ViewComponents
{

    public class SerieViewComponent : ViewComponent
    {
        private AppDbContext db;
        public SerieViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public IViewComponentResult Invoke()
        {
            var series = db.Series
                .Include(s => s.Season) // تحميل المواسم المرتبطة
                    .ThenInclude(season => season.Episode) // تحميل الحلقات المرتبطة
                .OrderByDescending(x => x.SerieId)
                .ToList();

            return View(series);
        }

    }
}
