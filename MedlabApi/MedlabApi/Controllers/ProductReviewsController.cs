using AutoMapper;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.ProductReviewDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="Admin, SuperAdmin")]

    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductReviewRepository _productReviewRepository;

        public ProductReviewsController(IMapper mapper, IProductReviewRepository productReviewRepository)
        {
            _mapper = mapper;
            _productReviewRepository = productReviewRepository;
        }


        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var reviews = _productReviewRepository.GetAll(x => true, "Product", "AppUser").OrderByDescending(x => !x.IsApproved).ThenByDescending(x => x.CreatedAt).ToList();
            if (reviews == null)
                return NotFound();

            List<ProductReviewGetDto> dto = _mapper.Map<List<ProductReviewGetDto>>(reviews);


            return Ok(dto);

        }
        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _productReviewRepository.GetAsync(x => x.Id == id, "Product", "AppUser");
            if (review == null)
                return NotFound();

            var dto = _mapper.Map<ProductReviewGetDto>(review);

            return Ok(dto);
        }

        //=========================
        // Delete
        //=========================
        [HttpDelete("Reject/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _productReviewRepository.GetAsync(x => x.Id == id);
            if (review == null)
                return NotFound();

            _productReviewRepository.Delete(review);
            _productReviewRepository.Commit();
            return NoContent();
        }

        //=========================
        // Approve
        //=========================
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var review = await _productReviewRepository.GetAsync(x => x.Id == id);
            if (review == null)
                return NotFound();

            review.IsApproved = true;
            review.UpdatedAt = DateTime.UtcNow.AddHours(4);
            _productReviewRepository.Commit();

            var dto = _mapper.Map<ProductReviewGetDto>(review);


            return Ok(dto);
        }

    }
}
