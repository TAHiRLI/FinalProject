namespace MedlabApi.Dtos.ProductDtos
{
    public class ProductListItemDto
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
        public string ImageUrl { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? ProductCategoryId { get; set; }

        public ProductCategoryInProductListItemDto? ProductCategory { get; set; }
    }
    public class ProductCategoryInProductListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
