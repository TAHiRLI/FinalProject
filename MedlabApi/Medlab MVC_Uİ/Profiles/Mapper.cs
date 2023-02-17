using AutoMapper;
using Medlab.Core.Entities;
using Medlab_MVC_Uİ.ViewModels;

namespace Medlab_MVC_Uİ.Profiles
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AppUser, EditProfileViewModel>().ReverseMap();
            CreateMap<ProductReviewViewModel, ProductReview>().ReverseMap();
            CreateMap<BasketItemViewModel, BasketItem>().ReverseMap();
        }
       
    }
}
