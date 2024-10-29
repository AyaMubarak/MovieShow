using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieShow;
using MovieShow.Data;
using MovieShow.Models;

namespace MovieShow.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Movies.ToListAsync();
            var model = products.Select(p => new Movie
            {
                MovieId = p.MovieId,
                Title = p.Title,
                Description = p.Description,
                Img = p.Img, // Ensure this is a valid image path
                Genre = p.Genre,
                Director = p.Director,
                ReleaseDate = p.ReleaseDate,
                Cast = p.Cast,
                TrailerUrl = p.TrailerUrl,
                VideoUrl = p.VideoUrl,
                Rating = p.Rating,
                IsDeleted = p.IsDeleted,
                Seasons = p.Seasons,
            }).ToList(); // Convert to List

            return View(model);
        }


        public IActionResult Details(int id)
        {
            var Post = _context.Movies.Find(id);
            if (Post == null)
            {
                return NotFound();
            }

            var viewModel = new Movie
            {
                MovieId = Post.MovieId,
                Title = Post.Title,
                Description = Post.Description,
                Img = Post.Img,
                Genre = Post.Genre,
                Director = Post.Director,
                ReleaseDate = Post.ReleaseDate,
                Cast = Post.Cast,
                TrailerUrl = Post.TrailerUrl,
                VideoUrl = Post.VideoUrl,
                Rating = Post.Rating,
                IsDeleted = Post.IsDeleted,
                Seasons = Post.Seasons,
            };

            return View(viewModel);
        }
    }
}
