using Medlab.Core.Entities;

namespace MedlabApi.Dtos.OrderDtos
{
    public class OrderGetDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string? Note { get; set; }
        public bool? OrderStatus { get; set; }
        public string? AppUserId { get; set; }
        public AppUserInOrderGetDto? AppUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemInOrderGetDto> OrderItems { get; set; } 

    }
    public class AppUserInOrderGetDto
    {
        public string  Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
    }
    public class OrderItemInOrderGetDto
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public ProductInOrderItem? Product { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public int Count { get; set; }

    }
    public class ProductInOrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; }
        public int AvgRating { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal DiscoutPercent { get; set; }
        public bool IsFeatured { get; set; }
        public bool StockStatus { get; set; }
        public bool IsSoldIndividual { get; set; }
        public string? ImageUrl { get; set; }
        public string? Link { get; set; }
    }
}
