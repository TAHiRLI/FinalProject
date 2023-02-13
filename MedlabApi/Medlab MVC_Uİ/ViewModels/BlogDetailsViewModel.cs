using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class BlogDetailsViewModel
    {
        public Blog Blog { get; set; }
        public List<Blog> RecentBlogs { get; set; } = new List<Blog>();
        public List<BlogCategory> BlogCategories { get; set; } = new List<BlogCategory>();
    }
}
