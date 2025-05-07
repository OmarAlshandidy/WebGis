using System.Threading.Tasks;
using AutoMapper;
using Gis.BLL.UnitOfWork;
using Gis.DAL.Models;
using Gis.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Gis.PL.Controllers
{
    public class MosquesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MosquesController(IUnitOfWork unitOfWork,IMapper  mapper)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string SearchInput)
        {

            IEnumerable<Mosque> mosques;
            if (string.IsNullOrEmpty(SearchInput))
            {
                mosques = await _unitOfWork.MosqueRepository.GetAllAsync();
            }
            else
            {
                mosques = await _unitOfWork.MosqueRepository.GetByNameAsync(SearchInput);
            }
            return View(mosques);

        }
        [HttpGet]
        public async Task<ActionResult> Search(string SearchInput)
        {
            IEnumerable<Mosque> mosques;
            if (string.IsNullOrEmpty(SearchInput))
            {
                mosques = await _unitOfWork.MosqueRepository.GetAllAsync();
            }
            else
            {
                mosques = await _unitOfWork.MosqueRepository.GetByNameAsync(SearchInput);
            }
            return PartialView("MosquePartialView/MosqueTablePartialView", mosques);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MosqueDto model)
        {
            if (ModelState.IsValid)
            {
                var mosques = _mapper.Map<Mosque>(model);
                await _unitOfWork.MosqueRepository.AddAsync(mosques);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Mosque Is Created !!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invaliad Id");
            var mosque = await _unitOfWork.MosqueRepository.GetAsync(id.Value);
            if (mosque is null) return NotFound(new { StatusCode = 404, Message = $"Mosque With Id {id} is not Found " });
            var mosqueDto = _mapper.Map<MosqueDto>(mosque);
            return View(ViewName, mosqueDto);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
           
            return await Details(id,"Edit");  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, MosqueDto model)
        {
            if (ModelState.IsValid)
            {
               
                var mosque = _mapper.Map<Mosque>(model);
                mosque.Objectid = id;
                _unitOfWork.MosqueRepository.Update(mosque);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id,  MosqueDto model)
        {
            if (ModelState.IsValid)
            {
                var mosque = _mapper.Map<Mosque>(model);
                mosque.Objectid = id;
                _unitOfWork.MosqueRepository.Delete(mosque);
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

