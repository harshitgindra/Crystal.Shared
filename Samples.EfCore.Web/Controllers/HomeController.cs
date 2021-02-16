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

        public IActionResult Index()
        {
            return View();
        }
    }
}
