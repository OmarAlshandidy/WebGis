using AutoMapper;
using Gis.BLL.UnitOfWork;
using Gis.DAL.Models;
using Gis.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gis.PL.Controllers
{
    [Authorize]
    public class MarketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MarketController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string SearchInput)
        {

            IEnumerable<Market> Markets;
            if (string.IsNullOrEmpty(SearchInput))
            {
                Markets = await _unitOfWork.MarketRepository.GetAllAsync();
            }
            else
            {
                Markets = await _unitOfWork.MarketRepository.GetByNameAsync(SearchInput);
            }
            return View(Markets);

        }
        [HttpGet]
        public async Task<ActionResult> Search(string SearchInput)
        {
            IEnumerable<Market> Markets;
            if (string.IsNullOrEmpty(SearchInput))
            {
                Markets = await _unitOfWork.MarketRepository.GetAllAsync();
            }
            else
            {
                Markets = await _unitOfWork.MarketRepository.GetByNameAsync(SearchInput);
            }
            return PartialView("MarketPartialView/MarketTablePartialView", Markets);
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MarketDto model)
        {
            if (ModelState.IsValid)
            {
                var Markets = _mapper.Map<Market>(model);
                await _unitOfWork.MarketRepository.AddAsync(Markets);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Market Is Created !!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invaliad Id");
            var Market = await _unitOfWork.MarketRepository.GetAsync(id.Value);
            if (Market is null) return NotFound(new { StatusCode = 404, Message = $"Market With Id {id} is not Found " });
            var MarketDto = _mapper.Map<MarketDto>(Market);
            return View(ViewName, MarketDto);
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]


        public async Task<IActionResult> Edit(int? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, MarketDto model)
        {
            if (ModelState.IsValid)
            {

                var Market = _mapper.Map<Market>(model);
                Market.Objectid = id;
                _unitOfWork.MarketRepository.Update(Market);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, MarketDto model)
        {
            if (ModelState.IsValid)
            {
                var Market = _mapper.Map<Market>(model);
                Market.Objectid = id;
                _unitOfWork.MarketRepository.Delete(Market);
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

