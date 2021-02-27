﻿using System;
using System.Linq;
using Crystal.Shared;
using Crystal.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Crystal.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Samples.EfCore.Web.Controllers
{
    public class DatatableController : Controller
    {
        private readonly IBaseUowRepository _uowRepository;

        public DatatableController(IBaseUowRepository uowRepository)
        {
            _uowRepository = uowRepository;
        }

        public async Task<IActionResult> Index()
        {
            await _Datasetup();
            return View();
        }

        public async Task<JsonResult> GetAll(DataTableRequest<Author> request)
        {
            try
            {
                var data = (await _uowRepository.Repository<Author>().QueryAsync())
                    .ToDatatable(request);
                return Json(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task _Datasetup()
        {
            if (!await _uowRepository.Repository<Author>().AnyAsync())
            {
                await _uowRepository.BeginTransactionAsync();
                await _uowRepository.Repository<Author>().InsertAsync(new Author()
                {
                    Age = 10,
                    BooksPublished = 5,
                    Country = "Aus",
                    Name = "Author 1"
                });
                await _uowRepository.Repository<Author>().InsertAsync(new Author()
                {
                    Age = 10,
                    BooksPublished = 21,
                    Country = "UK",
                    Name = "Author 2"
                });
                await _uowRepository.Repository<Author>().InsertAsync(new Author()
                {
                    Age = 10,
                    BooksPublished = 5,
                    Country = "IND",
                    Name = "Author 3"
                });
                var data = await _uowRepository.Repository<Book>().GetAsync();

                await _uowRepository.CommitAsync();
            }
        }
    }
}
