namespace MedlabApi.Dtos.UserDtos
{
    public class UserGetDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? ConnectionId { get; set; }
        public int CountOfOrders { get; set; }
        public string Role { get; set; }
    }
}
