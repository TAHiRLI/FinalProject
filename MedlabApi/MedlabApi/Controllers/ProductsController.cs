using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.ProductCategoryDtos;
using MedlabApi.Dtos.ProductDtos;
using MedlabApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IProductImageRepository _productImageRepository;

        public ProductsController
            (IMapper mapper,
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IWebHostEnvironment env,
            IProductImageRepository productImageRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _env = env;
            _productImageRepository = productImageRepository;
        }


        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var products = _productRepository.GetAll(x => true, "ProductCategory", "ProductImages").OrderByDescending(x=> x.CreatedAt).ToList();
            List<ProductListItemDto> dto = _mapper.Map<List<ProductListItemDto>>(products); 

            return Ok(dto);
        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepository.GetAsync(x => x.Id == id, "ProductCategory", "ProductImages");
            if(product == null)
                return NotFound();
            ProductGetDto dto = _mapper.Map<ProductGetDto>(product);

            return Ok(dto);
        }

        //=========================
        // Create
        //=========================
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductPostDto dto )
        {
            if(!_productCategoryRepository.Any(x=> x.Id == dto.ProductCategoryId))
            {
                return BadRequest(new {errors= new {ProductCategoryId= new[] { "This Category Does Not Exist" } }});
            }
            Product product = _mapper.Map<Product>(dto);

                


            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");


            // save Product Images
            ProductImage productImage = new ProductImage
            {
                ImageUrl = FileManager.Save(dto.PosterImage, imagePath, "Uploads/Products", 200),
                IsMain = true,
            };
            product.ProductImages.Add(productImage);

            if(dto.OtherImages != null)
            {
                foreach (var imgFile in dto.OtherImages)
                {
                    ProductImage productImg = new ProductImage
                    {
                        ImageUrl = FileManager.Save(imgFile, imagePath, "Uploads/Products", 200),
                        IsMain = false,
                    };
                    product.ProductImages.Add(productImg);
                }
            }


              
            await _productRepository.AddAsync(product);
            await _productRepository.CommitAsync();


            return Ok(product);
        }
        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductPutDto dto)
        {
            var product =await _productRepository.GetAsync(x => x.Id == id, "ProductImages", "ProductCategory");
            if (product == null)
                return NotFound();

            if (dto.ProductCategoryId != null)
            {
                var category = await _productCategoryRepository.GetAsync(x=> x.Id == dto.ProductCategoryId);
                if (category == null)
                    return BadRequest(new { errors = new { ProductCategoryId = new[] { "This Category Does Not Exist" } } });
                product.ProductCategory = category; 
            }

            product.Name = dto.Name;
            product.Desc = dto.Desc;
            product.CostPrice = dto.CostPrice;
            product.SalePrice = dto.SalePrice;
            product.DiscoutPercent = dto.DiscountPercent;
            product.IsFeatured = dto.IsFeatured;
            product.StockStatus = dto.StockStatus;
            product.IsSoldIndividual = dto.IsSoldIndividual;

            // path
            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");



            // Edit PosterImage
            if (dto.PosterImage != null)
            {
                //delete image 
                FileManager.Delete(imagePath, "Uploads/Products", product.ProductImages.FirstOrDefault(x => x.IsMain == true).ImageUrl);
                product.ProductImages.Remove(product.ProductImages.FirstOrDefault(x => x.IsMain == true));

                ProductImage posterImage = new ProductImage
                {
                    IsMain = true,
                    ImageUrl = FileManager.Save(dto.PosterImage, imagePath, "Uploads/Products", 200)

                };
                   product.ProductImages.Add(posterImage);
            }

            // Add OtherImages
            if(dto.OtherImages != null)
            {
                foreach (var imageFile in dto.OtherImages)
                {

                    ProductImage otherImage = new ProductImage
                    {
                        IsMain = false,
                        ImageUrl = FileManager.Save(imageFile, imagePath, "Uploads/Products", 200)

                    };
                    product.ProductImages.Add(otherImage);
                }
            }

            _productCategoryRepository.Commit();

            var returnDto = _mapper.Map<ProductGetDto>(product);

            return Ok(returnDto);
        }

        //=========================
        // Delete Image
        //=========================
        [HttpDelete("Image/{id}")]
        public async Task<IActionResult> DeleteImage(int id  )
        {
            var image = await _productImageRepository.GetAsync(x => x.Id == id);
            if (image == null)
                return BadRequest(new { errors = new { OtherImages = new[] { "This Image Does Not Exist" } } });

            // path
            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            FileManager.Delete(imagePath, "Uploads/Products", image.ImageUrl);

            _productImageRepository.Delete(image);
            _productImageRepository.Commit();

            return NoContent();
        }

        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetAsync(x => x.Id == id,"ProductImages");
            if(product == null)
                return BadRequest(new { errors = new { Product = new[] { "This Porduct Does Not Exist" } } });

            // path
            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");


            foreach (var prImage in product.ProductImages)
            {
                FileManager.Delete(imagePath, "Uploads/Products", prImage.ImageUrl);
            }

            // path
            _productRepository.Delete(product);
            _productRepository.Commit();



            return NoContent();
        }
    }
}
