namespace MedlabApi.Dtos.ValueDtos
{
    public class ValueGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Desc { get; set; }
        public bool IsFeatured { get; set; }
    }
}
