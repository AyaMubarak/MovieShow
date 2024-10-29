using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShow.Data;
using MovieShow.Models;

[Area("Dashboard")]
public class SeriesController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SeriesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Display all series
    public async Task<IActionResult> Index()
    {
        var series = await _context.Series
            .Include(s => s.Season)
            .Where(s => !s.IsDeleted)
            .ToListAsync();
        return View(series);
    }

    // GET: Create a new series
    public IActionResult Create()
    {
        return View();
    }

    // GET: Series/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var serie = await _context.Series
            .Include(s => s.Season)
            .ThenInclude(season => season.Episode)
            .FirstOrDefaultAsync(s => s.SerieId == id);

        if (serie == null)
        {
            return NotFound();
        }

        return View(serie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Serie model, IFormFile img, List<Season> Seasons)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var serie = new Serie
        {
            Title = model.Title,
            Description = model.Description,
            Genre = model.Genre,
            Director = model.Director,
            Rating = model.Rating,
            Season = new List<Season>() // إنشاء قائمة جديدة للمواسم
        };

        foreach (var season in Seasons)
        {
            // تأكد من أن الموسم يحتوي على الحلقات
            season.Serie = serie; // ربط الموسم بالمسلسل

            foreach (var episode in season.Episode)
            {
                episode.SeasonId = season.SeasonId; // تعيين SeasonId للحلقة
                episode.Season = season; // ربط الحلقة بالموسم
            }

            serie.Season.Add(season);
        }

        if (img != null && img.Length > 0)
        {
            serie.Img = await UploadImage(img); // رفع الصورة
        }

        _context.Series.Add(serie);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }



    // GET: Edit an existing series
    public async Task<IActionResult> Edit(int id)
    {
        var serie = await _context.Series.Include(s => s.Season).FirstOrDefaultAsync(s => s.SerieId == id);
        if (serie == null)
        {
            return NotFound();
        }

        return View(serie);
    }

    // POST: Update the series
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Serie model, IFormFile img)
    {
        if (id != model.SerieId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var serie = await _context.Series.Include(s => s.Season).FirstOrDefaultAsync(s => s.SerieId == id);
        if (serie == null)
        {
            return NotFound();
        }

        // Update series properties
        serie.Title = model.Title;
        serie.Description = model.Description;
        serie.Genre = model.Genre;
        serie.Director = model.Director;
        serie.Rating = model.Rating;

        // Check for a new image upload
        if (img != null && img.Length > 0)
        {
            var imgPath = await UploadImage(img);
            if (imgPath != null)
            {
                serie.Img = imgPath;
            }
        }

        _context.Update(serie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Delete a series
    public async Task<IActionResult> Delete(int id)
    {
        var serie = await _context.Series.FindAsync(id);
        if (serie == null)
        {
            return NotFound();
        }

        return View(serie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var serie = await _context.Series.FindAsync(id);
        if (serie == null)
        {
            return NotFound(new { message = "Series not found." });
        }

        // Delete the image file if it exists
        await DeleteImage(serie.Img);

        // Remove the series from the database
        _context.Series.Remove(serie);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<string> UploadImage(IFormFile img)
    {
        if (img != null && img.Length > 0 && img.ContentType.StartsWith("image/"))
        {
            var fileName = Path.GetFileName(img.FileName);
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

        return null;
    }

    private async Task DeleteImage(string imgPath)
    {
        if (!string.IsNullOrEmpty(imgPath))
        {
            var imageFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPath.TrimStart('/'));
            if (System.IO.File.Exists(imageFullPath))
            {
                try
                {
                    System.IO.File.Delete(imageFullPath);
                }
                catch (Exception ex)
                {
                    // Log error (optional)
                }
            }
        }
    }
}
