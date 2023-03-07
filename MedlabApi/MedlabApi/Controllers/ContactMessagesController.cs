using AutoMapper;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.MessageDtos;
using MedlabApi.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace MedlabApi.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IConfiguration _configuration;

        public ContactMessagesController(IMapper mapper, IContactMessageRepository contactMessageRepository, IConfiguration configuration)
        {
            _mapper = mapper;
            _contactMessageRepository = contactMessageRepository;
            _configuration = configuration;
        }


        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var messages = _contactMessageRepository.GetAll(x => true, "AppUser").OrderByDescending(x => !x.IsReplied).ThenByDescending(x => x.CreatedAt);

            var dto = _mapper.Map<List<MessageGetDto>>(messages);

            return Ok(dto);

        }

        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await _contactMessageRepository.GetAsync(x => x.Id == id, "AppUser");
            if (message == null)
                return NotFound();

            var dto = _mapper.Map<MessageGetDto>(message);
            return Ok(dto);
        }

        //=========================
        // Reply
        //=========================
        [HttpPut("Reply/{id}")]
        public async Task<IActionResult> Reply(int id,[FromForm] MessageReplyDto dto)
        {
            var message = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (message == null)
                return NotFound();

            if(message.IsReplied != false)
                return BadRequest(new { errors = new { Message = new[] { "This Message has already been replied !!!"} } });

            EmailService emailService = new EmailService(_configuration);

            emailService.SendMail(message.Email, dto.Subject, dto.Text);

            message.IsReplied = true;
            message.UpdatedAt = DateTime.UtcNow.AddHours(4);
            _contactMessageRepository.Commit();


            return NoContent();

        }



    }
}
