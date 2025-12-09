using _2210900059_PhamTheQuyen_Lab03.Models;
using Microsoft.AspNetCore.Mvc;

namespace Day02.Controllers
{
    public class BookController : Controller
    {
        private Book book = new Book();
        public IActionResult Index()
        {
            ViewBag.authors = book.Authors;
            ViewBag.genres = book.Genres;

            var books = book.GetBookList();

            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.authors = book.Authors;
            ViewBag.genres = book.Genres;
            var b = new Book();
            return View(b);
        }
    }
}