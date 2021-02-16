using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Samples.EfCore.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.EfCore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUowRepository _uowRepository;

        public HomeController(IUowRepository uowRepository)
        {
            _uowRepository = uowRepository;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _uowRepository.Books.GetAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            return View(new Book());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            await _uowRepository.Books.InsertAsync(book);
            await _uowRepository.CommitAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _uowRepository.Books.FindAsync(id);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book model)
        {
            var book = await _uowRepository.Books.FindAsync(model.BookId);
            book.Name = model.Name;

            await _uowRepository.Books.UpdateAsync(book);
            await _uowRepository.CommitAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _uowRepository.Books.FindAsync(id);
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _uowRepository.Books.DeleteAsync(id);
            await _uowRepository.CommitAsync();

            return RedirectToAction("Index");
        }
    }
}
