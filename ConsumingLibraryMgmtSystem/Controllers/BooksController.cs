using ConsumingLibraryMgmtSystem.Models;
using ConsumingLibraryMgmtSystem.Services.Book;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace ConsumingLibraryMgmtSystem.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksServices _booksService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BooksController(IBooksServices booksService, IHttpContextAccessor httpContextAccessor)
        {
            _booksService = booksService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Dashboard
        [HttpGet]
        public async Task<IActionResult> Book()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

            var books = await _booksService.GetAllBooksAsync(token);
            var viewModel = new BookViewModel
            {
                Books = books.ToList()
            };
            return View(viewModel);
        }


        // POST: Add Book
        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel viewModel)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var result = await _booksService.AddBookAsync(new Books
                {
                    BookId = viewModel.BookId,
                    Title = viewModel.Title,
                    Genre = viewModel.Genre,
                    AuthorId = viewModel.AuthorId,
                    ISBN = viewModel.ISBN,
                    Quantity = viewModel.Quantity
                }, token);

                if (!result)
                {
                    ModelState.AddModelError("", "Error adding book.");
                }
            }

            return RedirectToAction("Book");
        }


        // POST: Edit Book
        [HttpPost]
        public async Task<IActionResult> EditBook(BookViewModel viewModel)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var result = await _booksService.UpdateBookAsync(new Books
                {
                    BookId = viewModel.BookId,
                    Title = viewModel.Title,
                    Genre = viewModel.Genre,
                    AuthorId = viewModel.AuthorId,
                    ISBN = viewModel.ISBN,
                    Quantity = viewModel.Quantity
                }, token);

                if (result != "Book updated successfully!")
                {
                    ModelState.AddModelError("", result);
                }
            }

            return RedirectToAction("Book");
        }


        // POST: Delete Book
        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Account");

            var result = await _booksService.DeleteBookAsync(id, token);

            if (result != "Book deleted successfully!")
            {
                ModelState.AddModelError("", result);
            }

            return RedirectToAction("Book");
        }

    }
}