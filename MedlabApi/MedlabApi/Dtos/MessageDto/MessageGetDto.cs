using Medlab.Core.Entities;

namespace MedlabApi.Dtos.MessageDto
{
    public class MessageGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string? AppUserId { get; set; }
        public AppUserInMessageGetDto? AppUser { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsReplied { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class AppUserInMessageGetDto
    {
        public string Id { get; set; }
        public string  UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
