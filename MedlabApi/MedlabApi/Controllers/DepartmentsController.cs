using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.Repositories;
using MedlabApi.Dtos.DepartmentDtos;
using MedlabApi.Dtos.ProductCategoryDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MedlabApi.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var departments = _departmentRepository.GetAll(x => true);

            var dto = _mapper.Map<List<DepartmentGetDto>>(departments);

            return Ok(dto);
        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentRepository.GetAsync(x => x.Id == id);
            if (department == null)
                return NotFound();


            var dto = _mapper.Map<DepartmentGetDto>(department);
            return Ok(dto);
        }

        //=========================
        // Create
        //=========================
        [HttpPost()]
        public async Task<IActionResult> Create(DepartmentPostDto dto)
        {
            Department department = _mapper.Map<Department>(dto);

            await _departmentRepository.AddAsync(department);
            await _departmentRepository.CommitAsync();

            return Ok(department);
        }

        //=========================
        // Edit
        //=========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, DepartmentPostDto dto)
        {
            var department = await _departmentRepository.GetAsync(x => x.Id == id);
            if (department == null)
                return NotFound();

            department.Name = dto.Name;
            _departmentRepository.Commit();

            var result = _mapper.Map<DepartmentPostDto>(department); ;

            return Ok(result);
        }

        //=========================
        // Delete
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentRepository.GetAsync(x => x.Id == id);
            if (department == null)
                return NotFound();

            _departmentRepository.Delete(department);
            _departmentRepository.Commit();

            return NoContent();
        }


    }
}
