using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShow.Models;
using MovieShow;
using MovieShow.Data;

namespace MovieShow.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Administrator/Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Administrator/Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                // Check if an image was uploaded
                if (img != null && img.Length > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(img.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }

                        // Assign the image path to the movie model
                        movie.Img = $"/images/{fileName}";
                    }
                    catch (Exception ex)
                    {
                        // Log the exception and add an error message
                        ModelState.AddModelError("", $"File upload failed: {ex.Message}");
                        return View(movie);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Image file is required.");
                    return View(movie);
                }

                // Add the movie to the context and save changes
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed; redisplay the form
            return View(movie);
        }


        // GET: Administrator/Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Administrator/Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, IFormFile img)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMovie = await _context.Movies.FindAsync(id);

                    if (existingMovie == null)
                    {
                        return NotFound();
                    }

                    // Update text fields
                    existingMovie.Title = movie.Title;
                    existingMovie.Description = movie.Description;
                    existingMovie.Genre = movie.Genre;
                    existingMovie.Director = movie.Director;
                    existingMovie.IsDeleted = movie.IsDeleted;
                    existingMovie.ReleaseDate = movie.ReleaseDate;
                    existingMovie.Cast = movie.Cast;
                 
                    existingMovie.TrailerUrl = movie.TrailerUrl;
                    existingMovie.VideoUrl = movie.VideoUrl;
                    existingMovie.Rating = movie.Rating;
                    existingMovie.Seasons = movie.Seasons;
                 
                    existingMovie.Img = movie.Img;

                    // Handle image upload
                    if (img != null && img.Length > 0)
                    {
                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(existingMovie.Img))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Path.GetFileName(existingMovie.Img));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var newFileName = Path.GetFileName(img.FileName);
                        var newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);
                        using (var stream = new FileStream(newImagePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }

                        existingMovie.Img = $"/files/images/{newFileName}";
                    }

                    _context.Update(existingMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Administrator/Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    return NotFound(new { message = "Movie not found." });
                }

                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(movie.Img))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Path.GetFileName(movie.Img));
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(500, new { message = $"Error deleting image file: {ex.Message}" });
                        }
                    }
                }

                // Remove the movie from the database
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Movie and its image deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is set up in the application)
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }


        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
