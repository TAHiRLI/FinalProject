using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class BlogViewModel
    {
        public List<Blog> Blogs { get; set; } = new List<Blog>();
        public List<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();
        public List<Blog> RecentBlogs { get; set; } = new List<Blog>();


    }
}
