using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.AmenityImageDtos;
using MedlabApi.Dtos.ValueDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValueRepository _valueRepository;

        public ValuesController(IMapper mapper, IValueRepository valueRepository)
        {
            _mapper = mapper;
            _valueRepository = valueRepository;
        }
        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var values = _valueRepository.GetAll(x => true);

            var dto = _mapper.Map<List<ValueGetDto>>(values);
            return Ok(dto);
        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _valueRepository.GetAsync(x => x.Id == id);
            if (value == null)
                return NotFound();

            var dto = _mapper.Map<ValueGetDto>(value);  

            return Ok(dto);
        }

        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create([FromForm] ValuePostDto dto)
        {
            Value value = _mapper.Map<Value>(dto);

            await _valueRepository.AddAsync(value);
            await _valueRepository.CommitAsync();

            var result = _mapper.Map<ValueGetDto>(value);
            return Ok(result);
        }

        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] ValuePutDto dto)
        {
            var value = await _valueRepository.GetAsync(x => x.Id == id);
            if (value == null)
                return NotFound();

            value.Name = dto.Name;
            value.Desc = dto.Desc;
            value.Icon = dto.Icon;
            value.IsFeatured = dto.IsFeatured;

            var result = _mapper.Map<ValueGetDto>(value);
            _valueRepository.Commit();

            return Ok(result);
        }

        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var value = await _valueRepository.GetAsync(x => x.Id == id);
            if (value == null)
                return NotFound();

            _valueRepository.Delete(value);
            _valueRepository.Commit();

            return NoContent();
        }


    }
}
