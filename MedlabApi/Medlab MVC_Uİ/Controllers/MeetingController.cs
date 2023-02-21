using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.Hubs;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Medlab_MVC_Uİ.Controllers
{
        [Authorize(Roles ="Doctor, Member")]
    public class MeetingController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHubContext<MeetingHub> _hubContext;

        public MeetingController(IDoctorRepository doctorRepository, IHubContext<MeetingHub> hubContext)
        {
            _doctorRepository = doctorRepository;
            _hubContext = hubContext;
        }
        public IActionResult Index( int doctorId, int appointmentId)
        {
            var roomId = Guid.NewGuid().ToString();
            return RedirectToAction("Room", new { roomId = roomId, doctorId = doctorId, appointmentId = appointmentId });
        }


        [HttpGet("meeting/{roomId}")]
        public async Task<IActionResult>  Room(string roomId, int doctorId, int appointmentId)
        {
            ViewBag.roomId = roomId;
            ViewBag.doctorId = doctorId;

            var doctor = await _doctorRepository.GetAsync(x => x.Id == doctorId, "AppUser");
            if (doctor == null)
                return NotFound();



            CallRoomViewModel model = new CallRoomViewModel
            {
                Doctor = doctor,
                AppointmentId = appointmentId
            };

            if (!User.IsInRole("Doctor"))
            {
                if (doctor.AppUser.ConnectionId == null)
                    return NotFound();
            await _hubContext.Clients.Client(doctor.AppUser.ConnectionId).SendAsync("RecieveRing", roomId, doctorId, appointmentId);
            }


            return View(model);
        }
    }
}
