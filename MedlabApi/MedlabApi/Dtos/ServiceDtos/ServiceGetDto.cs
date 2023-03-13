namespace MedlabApi.Dtos.ServiceDtos
{
    public class ServiceGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DetailedDesc { get; set; }
        public string ImageUrl { get; set; }
        public string Icon { get; set; }
        public bool isFeatured { get; set; }
    }
}
