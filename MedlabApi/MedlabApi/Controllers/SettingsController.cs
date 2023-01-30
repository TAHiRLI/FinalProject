using AutoMapper;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.SettingDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="SuperAdmin, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IMapper _mapper;

        public SettingsController(ISettingRepository settingRepository, IMapper mapper)
        {
            this._settingRepository = settingRepository;
            this._mapper = mapper;
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var Settings = _settingRepository.GetAll(x=> true).ToList();

            var dto = _mapper.Map<List<SettingGetDto> >(Settings);

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var setting =await _settingRepository.GetAsync(x => x.Id == id);
            if(setting == null)
                return NotFound();
            var dto = _mapper.Map<SettingGetDto>(setting);

            return Ok(dto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>  Edit(int id, SettingPutDto dto)
        {
            var setting =await _settingRepository.GetAsync(x => x.Id == id);
            if (setting == null)
                return NotFound();

            setting.Value = dto.Value;
            _settingRepository.Commit();
            return Ok();
        }
    }
}
