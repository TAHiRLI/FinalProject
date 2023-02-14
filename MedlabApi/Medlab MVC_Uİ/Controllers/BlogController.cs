using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Medlab_MVC_Uİ.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepostiory _blogRepostiory;
        private readonly IBlogCategoryRepository _blogCategoryRepository;

        public BlogController(IBlogRepostiory blogRepostiory, IBlogCategoryRepository blogCategoryRepository)
        {
            this._blogRepostiory = blogRepostiory;
            this._blogCategoryRepository = blogCategoryRepository;
        }
        public IActionResult Index(int? BlogCategoryId = null)
        {
            BlogViewModel model = new BlogViewModel();

            if (BlogCategoryId == null)
            {
                model.Blogs = _blogRepostiory.GetAll(x => true, "Doctor").OrderByDescending(x => x.CreatedAt).ToList();
            }
            else
            {
                if (BlogCategoryId != null && !_blogCategoryRepository.Any(x => x.Id == BlogCategoryId))
                {
                    return NotFound();
                }
                else
                {
                    model.Blogs = _blogRepostiory.GetAll(x => x.BlogCategoryId == BlogCategoryId).ToList();
                }

            }

            model.BlogCategories = _blogCategoryRepository.GetAll(x => true).ToList();
            model.RecentBlogs = _blogRepostiory.GetAll(x => true, "Doctor").OrderByDescending(x => x.CreatedAt).Take(3).ToList();


            ViewBag.BlogCategoryId = BlogCategoryId;
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult>  Details(int id)
        {

            var blog = await _blogRepostiory.GetAsync(x => x.Id == id, "Doctor");
            if (blog == null) return NotFound();

            BlogDetailsViewModel model = new BlogDetailsViewModel
            {
                Blog = blog,
                BlogCategories = _blogCategoryRepository.GetAll(x => true).ToList(),
                RecentBlogs = _blogRepostiory.GetAll(x => true).OrderByDescending(x => x.CreatedAt).Take(3).ToList(),
            };


            return View(model);

        }
    }
}
