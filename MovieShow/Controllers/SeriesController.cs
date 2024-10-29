using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShow.Data;
using MovieShow.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShow.Controllers
{
    public class SeriesController : Controller
    {
        private readonly AppDbContext _context;

        public SeriesController(AppDbContext context)
        {
            _context = context;
        }

        // Index action to list all series
        public async Task<IActionResult> Index()
        {
            var series = await _context.Series
                .Include(s => s.Season) // Include related seasons
                .ToListAsync();

            return View(series);
        }

        // Details action to view a single series with its seasons and episodes
        public async Task<IActionResult> Details(int id)
        {
            // استرداد المسلسل المحدد مع المواسم والحلقات
            var serie = await _context.Series
                .Include(s => s.Season) // تضمين المواسم
                    .ThenInclude(se => se.Episode) // تضمين الحلقات لكل موسم
                .FirstOrDefaultAsync(s => s.SerieId == id); // استرداد المسلسل حسب المعرف

            if (serie == null)
            {
                return NotFound(); // في حال عدم وجود المسلسل
            }

            var combineModel = new Combine
            {
                Serie = new List<Serie> { serie }, // إضافة المسلسل إلى النموذج
                Season = serie.Season.ToList(), // إضافة المواسم
                Episode = serie.Season.SelectMany(se => se.Episode).ToList() // إضافة جميع الحلقات
            };

            return View(combineModel);
        }


    }
}
