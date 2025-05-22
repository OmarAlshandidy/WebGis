using System.Diagnostics;
using System.Threading.Tasks;
using Gis.BLL.UnitOfWork;
using Gis.DAL.Models;
using Gis.PL.Dtos;
using Gis.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gis.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger ,IUnitOfWork unitOfWork)
        {
            _logger = logger;
          _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var viewModel = new ServicesViewModel
            {
                Mosques = (List<Mosque>)await _unitOfWork.MosqueRepository.GetAllAsync(),
                Pharmacies = (List<Pharmacy>)await _unitOfWork.PharmacyRepository.GetAllAsync(),
                Restaurants = (List<Restaurant>)await _unitOfWork.RestaurantRepository.GetAllAsync(),
                StudentHousings = (List<StudentHousing>)await _unitOfWork.StudentHousingRepository.GetAllAsync(),
                Markets = (List<Market>)await _unitOfWork.MarketRepository.GetAllAsync()

            };


            return View(viewModel);
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
    }
}
