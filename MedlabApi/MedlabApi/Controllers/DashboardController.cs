using Medlab.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedlabApi.Controllers
{
    //[Authorize(Roles ="Admin, SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDoctorAppointmentRepository _doctorAppointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public DashboardController(
            IProductCategoryRepository productCategoryRepository, 
            IProductRepository productRepository, 
            IProductReviewRepository productReviewRepository,
            IOrderRepository orderRepository, 
            IDoctorAppointmentRepository doctorAppointmentRepository,
            IDoctorRepository doctorRepository)
        {
            _orderRepository = orderRepository;
            _doctorAppointmentRepository = doctorAppointmentRepository;
            _doctorRepository = doctorRepository;
            _productRepository = productRepository;
            _productReviewRepository = productReviewRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        //=========================
        // Get Top Product categories
        //=========================

        [HttpGet("TopProductCategories")]
        public IActionResult GetProductCategories()
        {
            var categories = _productCategoryRepository.GetTopSoldCategory().Take(3).ToList();

            return Ok(categories);
        }

        //=========================
        // Get Top Products
        //=========================

        [HttpGet("TopProducts")]
        public IActionResult GetTopProducts()
        {
            var products = _productRepository.GetTopSoldProducts().Take(6).ToList();
            return Ok(products);
        }

        //=========================
        // Get Reviews By Month
        //=========================

        [HttpGet("ReviewsByMonth")]
        public IActionResult GetReviewsByMonth()
        {
            var ALlReviews = _productReviewRepository.GetAllReviewsByMonth();
            var RejectedReviews = _productReviewRepository.GetRejectedReviewsByMonth();
            var ApprovedReviews = _productReviewRepository.GetApprovedReviewsByMonth();
            return Ok(new {Rejected = RejectedReviews, Approved= ApprovedReviews, AllReviews = ALlReviews});
        }

        //=========================
        // Get Sale Income By Month
        //=========================

        [HttpGet("SalesByMonth")]
        public IActionResult GetSalesByMonth()
        {
            var sales = _orderRepository.GetSalesByMonth();
            return Ok(sales);
        }

        //=========================
        // Get Appintment Income By Month
        //=========================

        [HttpGet("AppointmentPaymentsByMonth")]
        public IActionResult GetPaymentsByMonth()
        {
            var payments = _doctorAppointmentRepository.GetAppointmentPaymentByMonth();

            return Ok(payments);
        }

        //=========================
        // Get Top Doctors
        //=========================

        [HttpGet("TopDoctors")]
        public IActionResult GetTopDoctor()
        {
            var doctors = _doctorRepository.GetTopDoctor().Take(6).ToList();
            return Ok(doctors);
        }

        //=========================
        // Get Sales Summary
        //=========================

        [HttpGet("SalesSummary")]
        public IActionResult GetSalesSummary()
        {
            var salesSummary = _orderRepository.GetSalesSummary();
            return Ok(salesSummary);
        }
    }
}
