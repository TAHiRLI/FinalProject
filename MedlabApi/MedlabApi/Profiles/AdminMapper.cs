using AutoMapper;
using Medlab.Core.Entities;
using MedlabApi.Dtos.SettingDtos;

namespace MedlabApi.Profiles
{
    public class AdminMapper:Profile
    {
        public AdminMapper(IHttpContextAccessor httpAccessor)
        {
            CreateMap<Setting, SettingGetDto>();
        }
    }
}
