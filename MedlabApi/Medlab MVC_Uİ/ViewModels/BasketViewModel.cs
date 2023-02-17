namespace Medlab_MVC_Uİ.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> BasketItems { get; set; } = new List<BasketItemViewModel>();
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
    }
}
