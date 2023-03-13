using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.Repositories;
using MedlabApi.Dtos.DoctorDtos;
using MedlabApi.Dtos.ServiceDtos;
using MedlabApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;
        private readonly IWebHostEnvironment _env;

        public ServicesController(IMapper mapper, IServiceRepository serviceRepository, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _env = env;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var services = _serviceRepository.GetAll(x => true);
            var dto = _mapper.Map<List<ServiceGetDto>>(services);

            return Ok(dto);
        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _serviceRepository.GetAsync(x => x.Id == id);
            if (service == null)
                return NotFound();

            var dto = _mapper.Map<ServiceGetDto>(service);

            return Ok(dto);
        }

        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] ServicePostDto dto)
        {
            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");
            Service service = new Service
            {
                Name = dto.Name,
                Description = dto.Description,
                DetailedDesc = dto.DetailedDesc,
                Icon = dto.Icon,
                isFeatured = dto.isFeatured,
                ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/Services", 200)
            };

            await _serviceRepository.AddAsync(service);
            await _serviceRepository.CommitAsync();

            var result = _mapper.Map<ServiceGetDto>(service);

            return Ok(result);
        }

        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] ServicePutDto dto)
        {
            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            var service = await _serviceRepository.GetAsync(x => x.Id == id);
            if (service == null)
                return NotFound();

            service.Name = dto.Name;
            service.Description = dto.Description;
            service.isFeatured = dto.isFeatured;
            service.DetailedDesc = dto.DetailedDesc;
            service.Icon = dto.Icon;

            if(dto.Image != null)
            {
                FileManager.Delete(imagePath, "Uploads/Services", service.ImageUrl);
                service.ImageUrl = FileManager.Save(dto.Image,imagePath,  "Uploads/Services", 200);
            }

            _serviceRepository.Commit();

            var result = _mapper.Map<ServiceGetDto>(service);
            return Ok(result);
        }

        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceRepository.GetAsync(x => x.Id == id);
            if (service == null)
                return NotFound();

    

            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");


            // delete doctor image 
            FileManager.Delete(imagePath, "Uploads/Services", service.ImageUrl);


            _serviceRepository.Delete(service);

            _serviceRepository.Commit();


            return NoContent();
        }
    }
}
