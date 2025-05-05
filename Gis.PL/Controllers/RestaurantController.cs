using AutoMapper;
using Gis.BLL.UnitOfWork;
using Gis.DAL.Models;
using Gis.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gis.PL.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RestaurantController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string SearchInput)
        {

            IEnumerable<Restaurant> Restaurants;
            if (string.IsNullOrEmpty(SearchInput))
            {
                Restaurants = await _unitOfWork.RestaurantRepository.GetAllAsync();
            }
            else
            {
                Restaurants = await _unitOfWork.RestaurantRepository.GetByNameAsync(SearchInput);
            }
            return View(Restaurants);

        }
        [HttpGet]
        public async Task<ActionResult> Search(string SearchInput)
        {
            IEnumerable<Restaurant> Restaurants;
            if (string.IsNullOrEmpty(SearchInput))
            {
                Restaurants = await _unitOfWork.RestaurantRepository.GetAllAsync();
            }
            else
            {
                Restaurants = await _unitOfWork.RestaurantRepository.GetByNameAsync(SearchInput);
            }
            return PartialView("RestaurantPartialView/RestaurantTablePartialView", Restaurants);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RestaurantDto model)
        {
            if (ModelState.IsValid)
            {
                var Restaurants = _mapper.Map<Restaurant>(model);
                await _unitOfWork.RestaurantRepository.AddAsync(Restaurants);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Restaurant Is Created !!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invaliad Id");
            var Restaurant = await _unitOfWork.RestaurantRepository.GetAsync(id.Value);
            if (Restaurant is null) return NotFound(new { StatusCode = 404, Message = $"Restaurant With Id {id} is not Found " });
            var RestaurantDto = _mapper.Map<RestaurantDto>(Restaurant);
            return View(ViewName, RestaurantDto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, RestaurantDto model)
        {
            if (ModelState.IsValid)
            {

                var Restaurant = _mapper.Map<Restaurant>(model);
                Restaurant.Objectid = id;
                _unitOfWork.RestaurantRepository.Update(Restaurant);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, RestaurantDto model)
        {
            if (ModelState.IsValid)
            {
                var Restaurant = _mapper.Map<Restaurant>(model);
                Restaurant.Objectid = id;
                _unitOfWork.RestaurantRepository.Delete(Restaurant);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }

            }
            return View(model);
        }
    }
}

