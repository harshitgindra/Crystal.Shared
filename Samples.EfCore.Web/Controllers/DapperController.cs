using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Samples.EfCore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crystal.Abstraction;
using Crystal.Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.DbContext;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using Microsoft.Data.Sqlite;
using IBaseUowRepository = Crystal.Dapper.IBaseUowRepository;

namespace Samples.EfCore.Web.Controllers
{
    public class DapperController : Controller
    {
        private readonly IBaseUowRepository _ctx;

        public DapperController(IBaseUowRepository ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            //***
            //*** Get all books from the database
            //***
            var data = await _ctx.Repository<Book>().FindAllAsync();
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
            await _ctx.Repository<Book>().InsertAsync(book);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //***
            //*** Find the book from the database based on primary id
            //***
            var book = await _ctx.Repository<Book>().FindAsync(x => x.BookId == id);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book model)
        {
            var book = await _ctx.Repository<Book>().FindAsync(x => x.BookId == model.BookId);
            book.Name = model.Name;
            //***
            //*** Update book details in the database
            //***
            await _ctx.Repository<Book>().UpdateAsync(book);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _ctx.Repository<Book>().FindAsync(x => x.BookId == id);
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //***
            //*** Delete the book from the database based on primary id
            //***
            await _ctx.Repository<Book>().DeleteAsync(x => x.BookId == id);

            return RedirectToAction("Index");
        }
    }
}
