using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.Helpers;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace Medlab_MVC_Uİ.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepostiory _blogRepostiory;
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IWebHostEnvironment _env;

        public BlogController(IBlogRepostiory blogRepostiory, IBlogCategoryRepository blogCategoryRepository, UserManager<AppUser> userManager, IDoctorRepository doctorRepository, IWebHostEnvironment env)
        {
            this._blogRepostiory = blogRepostiory;
            this._blogCategoryRepository = blogCategoryRepository;
            _userManager = userManager;
            _doctorRepository = doctorRepository;
            _env = env;
        }

        //================================
        // Index
        //================================

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

            model.BlogCategories = _blogCategoryRepository.GetAll(x => x.Blogs.Count> 0).ToList();
            model.RecentBlogs = _blogRepostiory.GetAll(x => true, "Doctor").OrderByDescending(x => x.CreatedAt).Take(3).ToList();


            ViewBag.BlogCategoryId = BlogCategoryId;
            return View(model);
        }

        //================================
        // Details
        //================================
        public async Task<IActionResult>  Details(int id)
        {

            var blog = await _blogRepostiory.GetAsync(x => x.Id == id, "Doctor");
            if (blog == null) return NotFound();

            BlogDetailsViewModel model = new BlogDetailsViewModel
            {
                Blog = blog,
                BlogCategories = _blogCategoryRepository.GetAll(x => x.Blogs.Count>0).ToList(),
                RecentBlogs = _blogRepostiory.GetAll(x => true).OrderByDescending(x => x.CreatedAt).Take(3).ToList(),
            };


            return View(model);

        }

        //================================
        // Create Blog
        //================================

        [Authorize(Roles ="Doctor")]
        public IActionResult Create()
        {
            ViewBag.Categories = _blogCategoryRepository.GetAll(x => true).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel BlogVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _blogCategoryRepository.GetAll(x => true).ToList();
                return View(BlogVm);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            var doctor = await _doctorRepository.GetAsync(x => x.Id == user.DoctorId, "Blogs");
            if (doctor == null)
                return NotFound();

            Blog blog = new Blog
            {
                Title = BlogVm.Title,
                BlogCategoryId = BlogVm.CategoryId,
                ImageUrl = FileManager.Save(BlogVm.ImageFile, _env.WebRootPath ,"Assets/Uploads/Blogs",200),
                DoctorId = doctor.Id,
                Text = BlogVm.MainContent,
                PrevText = BlogVm.PrevContent,
                
            };

            doctor.Blogs.Add(blog);
            _doctorRepository.Commit();

            return RedirectToAction("profile", "account");
        }


        //================================
        // Edit Blog
        //================================

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult>  Edit(int id)
        {
            var blog = await _blogRepostiory.GetAsync(x => x.Id == id);
            if (blog == null)
                return NotFound();


            BlogEditViewModel Model = new BlogEditViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                CategoryId = blog.BlogCategoryId,
                MainContent = blog.Text,
                PrevContent = blog.PrevText
            };

            ViewBag.Categories = _blogCategoryRepository.GetAll(x => true).ToList();
            return View(Model);
        }


        [HttpPost]
        [Authorize(Roles = "Doctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogEditViewModel BlogVm)
        {
            var existBlog =await _blogRepostiory.GetAsync(x=> x.Id == BlogVm.Id);
            if (existBlog == null)
                return NotFound();



            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _blogCategoryRepository.GetAll(x => true).ToList();
                return View(BlogVm);
            }

            if(BlogVm.ImageFile  != null)
            {
                FileManager.Delete(_env.WebRootPath, "Assets/Uploads/Blogs", existBlog.ImageUrl);
                existBlog.ImageUrl = FileManager.Save(BlogVm.ImageFile, _env.WebRootPath, "Assets/Uploads/Blogs", 200);
            }
            existBlog.Title = BlogVm.Title;
            existBlog.BlogCategoryId = BlogVm.CategoryId;
            existBlog.Text = BlogVm.MainContent;
            existBlog.PrevText = BlogVm.PrevContent;

            _blogCategoryRepository.Commit();

            return RedirectToAction("Profile", "Account");
        }

    }
}
