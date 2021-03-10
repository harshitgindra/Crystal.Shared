using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Crystal.EntityFrameworkCore;

namespace Samples.EfCore.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBaseUowRepository _uowRepository;

        public BookController(IBaseUowRepository uowRepository)
        {
            _uowRepository = uowRepository;
        }

        public async Task<IActionResult> Index()
        {
            //***
            //*** Get all books from the database
            //***
            var data = await _uowRepository.Repository<Book>().GetAsync<BookViewModel>();
            return View(data);
        }

        public IActionResult Create()
        {
            return View(new Book());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            //***
            //*** Add/insert new book to the database
            //***
            await _uowRepository.Repository<Book>().InsertAsync(book);
            await _uowRepository.CommitAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //***
            //*** Find the book from the database based on primary id
            //***
            var book = await _uowRepository.Repository<Book>().FindAsync(id);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book model)
        {
            var book = await _uowRepository.Repository<Book>().FindAsync(model.BookId);
            book.Name = model.Name;
            //***
            //*** Update book details in the database
            //***
            await _uowRepository.Repository<Book>().UpdateAsync(book);
            await _uowRepository.CommitAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _uowRepository.Repository<Book>().FindAsync(id);
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //***
            //*** Delete the book from the database based on primary id
            //***
            await _uowRepository.Repository<Book>().DeleteAsync(id);
            await _uowRepository.CommitAsync();

            return RedirectToAction("Index");
        }
    }
}
