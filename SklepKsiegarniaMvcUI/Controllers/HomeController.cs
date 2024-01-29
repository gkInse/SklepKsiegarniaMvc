using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SklepKsiegarniaMvcUI.Models;
using SklepKsiegarniaMvcUI.Models.DTOs;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SklepKsiegarniaMvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sterm = "", int genreId = 0, int pageSize = 10)
        {
            HttpContext.Session.SetInt32("Visits", HttpContext.Session.GetInt32("Visits").GetValueOrDefault() + 1);
            ViewBag.VisitsCount = HttpContext.Session.GetInt32("Visits").GetValueOrDefault();

            IEnumerable<Book> books;
            if (!string.IsNullOrEmpty(sterm))
            {
                books = _homeRepository.SearchBooks(sterm);
            }
            else
            {
                books = await _homeRepository.GetBooks(sterm, genreId);
            }

            // Stronicowanie z uwzględnieniem wybranej liczby produktów na stronie
            books = books.Take(pageSize);

            IEnumerable<Genre> genres = await _homeRepository.Genres();
            BookDisplayModel bookModel = new BookDisplayModel
            {
                Books = books,
                Genres = genres,
                STerm = sterm,
                GenreId = genreId
            };

            return View(bookModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _homeRepository.GetBookById(id);
            return View(book);
        }
    }
}
