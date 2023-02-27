using FluentValidation;
using Medlab.Core.Entities;

namespace MedlabApi.Dtos.BlogDtos
{
    public class BlogGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string? PrevText { get; set; }
        public string ImageUrl { get; set; }
        public int? BlogCategoryId { get; set; }
        public BlogCategory? BlogCategory { get; set; }
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public string Link { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
 
}
