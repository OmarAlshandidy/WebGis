using AutoMapper;
using Gis.BLL.UnitOfWork;
using Gis.DAL.Models;
using Gis.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Gis.PL.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PharmacyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string SearchInput)
        {

            IEnumerable<Pharmacy> pharmacies;
            if (string.IsNullOrEmpty(SearchInput))
            {
                pharmacies = await _unitOfWork.PharmacyRepository.GetAllAsync();
            }
            else
            {
                pharmacies = await _unitOfWork.PharmacyRepository.GetByNameAsync(SearchInput);
            }
            return View(pharmacies);

        }
        [HttpGet]
        public async Task<ActionResult> Search(string SearchInput)
        {
            IEnumerable<Pharmacy> pharmacies;
            if (string.IsNullOrEmpty(SearchInput))
            {
                pharmacies = await _unitOfWork.PharmacyRepository.GetAllAsync();
            }
            else
            {
                pharmacies = await _unitOfWork.PharmacyRepository.GetByNameAsync(SearchInput);
            }
            return PartialView("PharmacyPartialView/PharmacyTablePartialView", pharmacies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PharmacyDto model)
        {
            if (ModelState.IsValid)
            {
                var pharmacies = _mapper.Map<Pharmacy>(model);
                await _unitOfWork.PharmacyRepository.AddAsync(pharmacies);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Pharmacy Is Created !!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invaliad Id");
            var Pharmacy = await _unitOfWork.PharmacyRepository.GetAsync(id.Value);
            if (Pharmacy is null) return NotFound(new { StatusCode = 404, Message = $"Pharmacy With Id {id} is not Found " });
            var PharmacyDto = _mapper.Map<PharmacyDto>(Pharmacy);
            return View(ViewName, PharmacyDto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, PharmacyDto model)
        {
            if (ModelState.IsValid)
            {

                var Pharmacy = _mapper.Map<Pharmacy>(model);
                Pharmacy.Objectid = id;
                _unitOfWork.PharmacyRepository.Update(Pharmacy);
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
        public async Task<IActionResult> Delete([FromRoute] int id, PharmacyDto model)
        {
            if (ModelState.IsValid)
            {
                var Pharmacy = _mapper.Map<Pharmacy>(model);
                Pharmacy.Objectid = id;
                _unitOfWork.PharmacyRepository.Delete(Pharmacy);
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
