using Crystal.Shared;
using Crystal.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Samples.EfCore.Web.Controllers
{
    public class DatatableController : Controller
    {
        private readonly IUowRepository _uowRepository;

        public DatatableController(IUowRepository uowRepository)
        {
            _uowRepository = uowRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll(DataTableRequest<Author> request)
        {
            var data = await _uowRepository.Authors.Entity.ToDatatableAsync(request);
            return Json(data);
        }
    }
}
