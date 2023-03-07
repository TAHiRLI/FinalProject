using AutoMapper;
using Medlab.Core.Repositories;
using MedlabApi.Dtos.OrderDtos;
using MedlabApi.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        public OrdersController(IMapper mapper, IOrderRepository orderRepository, IConfiguration configuration)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _configuration = configuration;
        }
        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var orders = _orderRepository.GetAll(x => true, "AppUser", "OrderItems").OrderByDescending(x => x.OrderStatus == null).ThenBy(x => x.CreatedAt).ToList();
            List<OrderGetDto> orderDto = _mapper.Map<List<OrderGetDto>>(orders);
            return Ok(orderDto);
        }
        //=========================
        // Get By id
        //=========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
                return NotFound();
            var dto = _mapper.Map<OrderGetDto>(order);
            return Ok(dto);
        }
        //=========================
        // Reject
        //=========================
        [HttpGet("Reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var order = await _orderRepository.GetAsync(x => x.Id == id);
            if (order == null)
                return NotFound();
            if (order.OrderStatus != null)
                return BadRequest();


            order.OrderStatus = false;
            order.UpdatedAt = DateTime.UtcNow.AddHours(4);

            EmailService emailService = new EmailService(_configuration);


            emailService.SendMail(order.Email, "Order Rejected", $"Your Order at {order.CreatedAt.ToString("dddd, dd MMMM yyyy")} is rejected");

            _orderRepository.Commit();

            return Ok();

        }
        //=========================
        // Approve
        //=========================
        [HttpGet("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var order = await _orderRepository.GetAsync(x => x.Id == id);
            if (order == null)
                return NotFound();
            if (order.OrderStatus != null)
                return BadRequest();

            order.OrderStatus = true;
            order.UpdatedAt = DateTime.UtcNow.AddHours(4);


            EmailService emailService = new EmailService(_configuration);

            emailService.SendMail(order.Email, "Order Rejected", $"Your Order at {order.CreatedAt.ToString("dddd, dd MMMM yyyy")} is Approved");

            _orderRepository.Commit();

            return Ok();

        }
    }
}
