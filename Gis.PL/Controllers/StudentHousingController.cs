using AutoMapper;
using Gis.BLL.UnitOfWork;
using Gis.DAL.Models;
using Gis.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gis.PL.Controllers
{
    public class StudentHousingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentHousingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string SearchInput)
        {

            IEnumerable<StudentHousing> StudentHousings;
            if (string.IsNullOrEmpty(SearchInput))
            {
                StudentHousings = await _unitOfWork.StudentHousingRepository.GetAllAsync();
            }
            else
            {
                StudentHousings = await _unitOfWork.StudentHousingRepository.GetByNameAsync(SearchInput);
            }
            return View(StudentHousings);

        }
        [HttpGet]
        public async Task<ActionResult> Search(string SearchInput)
        {
            IEnumerable<StudentHousing> StudentHousings;
            if (string.IsNullOrEmpty(SearchInput))
            {
                StudentHousings = await _unitOfWork.StudentHousingRepository.GetAllAsync();
            }
            else
            {
                StudentHousings = await _unitOfWork.StudentHousingRepository.GetByNameAsync(SearchInput);
            }
            return PartialView("StudentHousingPartialView/StudentHousingTablePartialView", StudentHousings);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(StudentHousingDto model)
        {
            if (ModelState.IsValid)
            {
                var StudentHousings = _mapper.Map<StudentHousing>(model);
                await _unitOfWork.StudentHousingRepository.AddAsync(StudentHousings);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "StudentHousing Is Created !!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invaliad Id");
            var StudentHousing = await _unitOfWork.StudentHousingRepository.GetAsync(id.Value);
            if (StudentHousing is null) return NotFound(new { StatusCode = 404, Message = $"StudentHousing With Id {id} is not Found " });
            var StudentHousingDto = _mapper.Map<StudentHousingDto>(StudentHousing);
            return View(ViewName, StudentHousingDto);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, StudentHousingDto model)
        {
            if (ModelState.IsValid)
            {

                var StudentHousing = _mapper.Map<StudentHousing>(model);
                StudentHousing.Objectid = id;
                _unitOfWork.StudentHousingRepository.Update(StudentHousing);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, StudentHousingDto model)
        {
            if (ModelState.IsValid)
            {
                var StudentHousing = _mapper.Map<StudentHousing>(model);
                StudentHousing.Objectid = id;
                _unitOfWork.StudentHousingRepository.Delete(StudentHousing);
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

