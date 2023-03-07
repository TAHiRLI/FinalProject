using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.DepartmentDtos;
using MedlabApi.Dtos.DoctorDtos;
using MedlabApi.Dtos.SliderDtos;
using MedlabApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MedlabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISliderRepository _sliderRepository;
        private readonly IWebHostEnvironment _env;

        public SlidersController(IMapper mapper, ISliderRepository sliderRepository, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _sliderRepository = sliderRepository;
            _env = env;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var sliders = _sliderRepository.GetAll(x => true).OrderBy(x => x.Order).ToList();

            var dto = _mapper.Map<List<SliderGetDto>>(sliders);
            return Ok(dto);

        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var slider = await _sliderRepository.GetAsync(x => x.Id == id);
            if (slider == null)
                return NotFound();

            var dto = _mapper.Map<SliderGetDto>(slider);
            return Ok(dto);
        }

        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] SliderPostDto dto)
        {
            var sliders = _sliderRepository.GetAll(x => true);
            var maxOrder = sliders.Max(x => x.Order);
            if (dto.Order < 0 || dto.Order > maxOrder + 1)
                return BadRequest(new { errors = new { Order = new[] { $"Invalid Order Value !!! \n Order must be between 0-{maxOrder + 1}" } } });


            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            Slider newSlider = _mapper.Map<Slider>(dto);
            newSlider.ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/Sliders", 200);

            if (dto.Order < maxOrder)
            {
                foreach (var slide in sliders.Where(x => x.Order >= dto.Order))
                {
                    slide.Order++;
                }
            }

            await _sliderRepository.AddAsync(newSlider);
            await _sliderRepository.CommitAsync();

            var result = _mapper.Map<SliderGetDto>(newSlider);

            return Ok(result);
        }

        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] SliderPutDto dto)
        {
            var slider = await _sliderRepository.GetAsync(x => x.Id == id);
            if (slider == null)
                return NotFound();

            var sliders = _sliderRepository.GetAll(x => true);
            var maxOrder = sliders.Max(x => x.Order);
            if (dto.Order < 0 || dto.Order > maxOrder)
                return BadRequest(new { errors = new { Order = new[] { $"Invalid Order Value !!! \n Order must be between 0-{maxOrder}" } } });

            if (dto.Order != slider.Order)
            {
                await UpdateSliderOrderAsync(id, dto.Order);
            }

            if (dto.Image != null)
            {
                var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
                var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

                FileManager.Delete(imagePath, "Uploads/Sliders", slider.ImageUrl);
                slider.ImageUrl = FileManager.Save(dto.Image, imagePath, "Uploads/Sliders", 200);
            }

            slider.Title = dto.Title;
            slider.Desc = dto.Desc;
            slider.BtnText = dto.BtnText;
            slider.BtnUrl = dto.BtnUrl;


            _sliderRepository.Commit();

            var result = _mapper.Map<SliderGetDto>(slider);

            return Ok(result);
        }

        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _sliderRepository.GetAsync(x => x.Id == id);
            if (slider == null)
                return NotFound();

            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");
            FileManager.Delete(imagePath, "Uploads/Sliders", slider.ImageUrl);

            _sliderRepository.Delete(slider);

            var sliders = _sliderRepository.GetAll(x => true).OrderBy(x=> x.Order);
            int index = 0;
            foreach (var slide in sliders.Where(x=> x.Id != id).ToList())
            {
                slide.Order = index++;
            }

            _sliderRepository.Commit();
            return NoContent();
        }
        //=========================
        // Functions
        //=========================

        private async Task<bool> UpdateSliderOrderAsync(int id, int newOrder)
        {
            var sliders = _sliderRepository.GetAll(x => true);
            var slider = await _sliderRepository.GetAsync(x => x.Id == id);
            if (slider == null)
                return false;

            var currentOrder = slider.Order;

            if (newOrder < currentOrder)
            {
                foreach (var slide in sliders.Where(x => x.Order >= newOrder && x.Order < currentOrder))
                {
                    slide.Order++;
                }
            }
            else if (newOrder > currentOrder)
            {
                foreach (var slide in sliders.Where(x => x.Order <= newOrder && x.Order > currentOrder))
                {
                    slide.Order--;
                }
            }

            slider.Order = newOrder;

            _sliderRepository.Commit();
            return true;
        }


    }
}
