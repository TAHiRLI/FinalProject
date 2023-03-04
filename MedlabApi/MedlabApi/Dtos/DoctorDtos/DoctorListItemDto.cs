using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MedlabApi.Dtos.DoctorDtos
{
    public class DoctorListItemDto
    {
        public int Id { get; set; }
        public int? DepartmentId { get; set; }
        public DepartmentInDoctorListItemDto? Department { get; set; }
        public string Positon { get; set; }
        public string Office { get; set; }
        public string Link { get; set; }
        public string Fullname { get; set; }
        public string ImageUrl { get; set; }
        public string Desc { get; set; }
        public string DetailedDesc { get; set; }
        public AppUserInDoctorListItemDto AppUser { get; set; }

        public decimal Salary { get; set; }
        public decimal MeetingPrice { get; set; }
        public bool Gender { get; set; }
        public bool IsFeatured { get; set; }
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string Twitter  { get; set; }
        public string Facebook { get; set; }
        public int BlogsCount { get; set; }
        public DateTime CreatedAt { get; set; }


    }
    public class DepartmentInDoctorListItemDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
       
    }
    public class AppUserInDoctorListItemDto
    {
        public string Id { get; set; }
        public string  Email { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
    }
}
