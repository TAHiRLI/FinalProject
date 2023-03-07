namespace MedlabApi.Dtos.SliderDtos
{
    public class SliderGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        public string BtnText { get; set; }
        public string BtnUrl { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
