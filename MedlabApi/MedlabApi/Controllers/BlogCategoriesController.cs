using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.Repositories;
using MedlabApi.Dtos.BlgoCategoriyDtos;
using MedlabApi.Dtos.ValueDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MedlabApi.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]

    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBlogCategoryRepository _blogCategoryRepository;

        public BlogCategoriesController(IMapper mapper, IBlogCategoryRepository blogCategoryRepository)
        {
            _mapper = mapper;
            _blogCategoryRepository = blogCategoryRepository;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var categories = _blogCategoryRepository.GetAll(x => true);
            var dto = _mapper.Map<List<BlogCategoryGetDto>>(categories);

            return Ok(dto);
        }


        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _blogCategoryRepository.GetAsync(x => x.Id == id);
            if (category == null)
                return NotFound();
            var dto = _mapper.Map<BlogCategoryGetDto>(category);
            return Ok(dto);
        }


        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create(BlogCategoryPostDto dto)
        {
            BlogCategory blogCategory = new BlogCategory
            {
                Name = dto.Name
            };

            await _blogCategoryRepository.AddAsync(blogCategory);
            await _blogCategoryRepository.CommitAsync();

            var result = _mapper.Map<BlogCategoryGetDto>(blogCategory);
            return Ok(result);
        }
        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, BlogCategoryPutDto dto)
        {
            var category = await _blogCategoryRepository.GetAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            category.Name = dto.Name;
            _blogCategoryRepository.Commit();
            var result = _mapper.Map<BlogCategoryGetDto>(category);
            return Ok(result);
        }

        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _blogCategoryRepository.GetAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            _blogCategoryRepository.Delete(category);
            _blogCategoryRepository.Commit();

            return NoContent();
        }
    }
}
