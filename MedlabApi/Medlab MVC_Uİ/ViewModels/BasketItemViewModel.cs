using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class BasketItemViewModel
    {
        public int id { get; set; } 
        public int Count { get; set; }
        public Product Product { get; set; }
    }
}
