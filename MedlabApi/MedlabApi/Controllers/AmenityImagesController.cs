using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.AmenityImageDtos;
using MedlabApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedlabApi.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityImagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAmenityImageRepository _amenityImageRepository;
        private readonly IHostEnvironment _env;

        public AmenityImagesController(IMapper mapper, IAmenityImageRepository amenityImageRepository, IHostEnvironment env)
        {
            _mapper = mapper;
            _amenityImageRepository = amenityImageRepository;
            _env = env;
        }
        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var images = _amenityImageRepository.GetAll(x => true);

            var dto = _mapper.Map<List<AmenityImageGetDto>>(images);

            return Ok(dto);

        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenityImage = await _amenityImageRepository.GetAsync(x => x.Id == id);
            if (amenityImage == null)
                return NotFound();

            var dto = _mapper.Map<AmenityImageGetDto>(amenityImage);
            return Ok(dto);
        }

        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] AmenityImagePostDto dto)
        {

            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            AmenityImage amenityImage = new AmenityImage
            {
                ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/AmenityImages", 200)
            };
            await _amenityImageRepository.AddAsync(amenityImage);
            await _amenityImageRepository.CommitAsync();
            var result = _mapper.Map<AmenityImageGetDto>(amenityImage);
            return Ok(result);
        }

        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] AmenityImagePutDto dto)
        {

            var amenityImage = await _amenityImageRepository.GetAsync(x=> x.Id == id);
            if (amenityImage == null)
                return NotFound();
            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            FileManager.Delete(imagePath, "Uploads/AmenityImages", amenityImage.ImageUrl);

            amenityImage.ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/AmenityImages", 200);

            _amenityImageRepository.Commit();

            var result = _mapper.Map<AmenityImageGetDto>(amenityImage);

            return Ok(result);
        }
        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var amenityImage = await _amenityImageRepository.GetAsync(x => x.Id == id);
            if (amenityImage == null)
                return NotFound();

            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");


            FileManager.Delete(imagePath, "Uploads/AmenityImages", amenityImage.ImageUrl);

            _amenityImageRepository.Delete(amenityImage);
            _amenityImageRepository.Commit();
            return NoContent();
        }
    }
}
