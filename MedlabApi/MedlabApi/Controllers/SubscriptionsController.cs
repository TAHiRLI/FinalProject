using Medlab.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedlabApi.Controllers
{
    [Authorize(Roles ="SuperAdmin, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionRepostiory _subscriptionRepostiory;

        public SubscriptionsController(ISubscriptionRepostiory subscriptionRepostiory)
        {
            _subscriptionRepostiory = subscriptionRepostiory;
        }

        //=========================
        // Get All
        //=========================

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var subscribes = _subscriptionRepostiory.GetAll(x => true);
            return Ok(subscribes);
        }

        //=========================
        // Delete 
        //=========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscribe =await _subscriptionRepostiory.GetAsync(x => x.Id == id);
            if (subscribe == null)
                return NotFound();

            _subscriptionRepostiory.Delete(subscribe);
            _subscriptionRepostiory.Commit();
            return NoContent();
        }

       
    }
}
