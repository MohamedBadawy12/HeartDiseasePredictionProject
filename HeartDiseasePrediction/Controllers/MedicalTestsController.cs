using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
    public class MedicalTestsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MedicalTestViewModel model)
        {
            return View(model);
        }
    }
}
