using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.ProductCategoryDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;

        public ProductCategoriesController(IProductCategoryRepository productCategoryRepository, IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var categories = _productCategoryRepository.GetAll(x => true);

            var dto = _mapper.Map<List<ProductCategoryGetDto>>(categories);

            return Ok(dto);
        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category =await _productCategoryRepository.GetAsync(x => x.Id==id);
            if (category == null)
                return NotFound();


            var dto = _mapper.Map<ProductCategoryGetDto>(category);
            return Ok(dto);
        }




        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create(ProductCategoryPostDto dto)
        {
            ProductCategory productCategory = _mapper.Map<ProductCategory>(dto);

            await _productCategoryRepository.AddAsync(productCategory);
            await _productCategoryRepository.CommitAsync();

            return Ok(productCategory);
        }
        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,ProductCategoryPostDto dto)
        {
            var category =await _productCategoryRepository.GetAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            category.Name = dto.Name;
            _productCategoryRepository.Commit();

            var result =  _mapper.Map<ProductCategoryPostDto>(category); ;

            return Ok(result);
        }

        //=========================
        // Delete
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _productCategoryRepository.GetAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            _productCategoryRepository.Delete(category);
            _productCategoryRepository.Commit();

            return NoContent();
        }

    }
}
