using AutoMapper;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.BlogDtos;
using MedlabApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBlogRepostiory _blogRepostiory;
        private readonly IWebHostEnvironment _env;

        public BlogsController( IMapper mapper, IBlogRepostiory blogRepostiory, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _blogRepostiory = blogRepostiory;
            _env = env;
        }
        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var blogs = _blogRepostiory.GetAll(x => true, "Doctor").OrderByDescending(x => x.CreatedAt).ToList() ;
            List<BlogGetDto> dto = _mapper.Map<List<BlogGetDto>>(blogs);

          
            return Ok(dto);
        }
        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var blog = await _blogRepostiory.GetAsync(x => x.Id == id, "Doctor");
            return Ok();
        }


        //=========================
        // Delete
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogRepostiory.GetAsync(x => x.Id == id, "Doctor");
            if (blog == null)
                return NotFound();

            // delete blog image 

            var mvcProjectDirectory = new DirectoryInfo(Path.Combine(_env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            FileManager.Delete(imagePath, "Uploads/Blogs", blog.ImageUrl);

            _blogRepostiory.Delete(blog);
            _blogRepostiory.Commit();
            

            return NoContent();
        }
    }
}
