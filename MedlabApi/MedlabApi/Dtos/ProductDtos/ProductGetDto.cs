using Medlab.Core.Entities;

namespace MedlabApi.Dtos.ProductDtos
{
    public class ProductGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int AvgRating { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal DiscoutPercent { get; set; }
        public bool IsFeatured { get; set; }
        public bool StockStatus { get; set; }
        public bool IsSoldIndividual { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? ProductCategoryId { get; set; }
        public string Link { get; set; }
        public List<ProductImageInProductGetDto> ProductImages { get; set; }

        public ProductCategoryInProductGetDto? ProductCategory { get; set; }
    }
    public class ProductCategoryInProductGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProductImageInProductGetDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
