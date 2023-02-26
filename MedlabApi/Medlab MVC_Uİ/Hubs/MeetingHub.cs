using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Medlab_MVC_Uİ.Hubs
{
    //============================
    // set start and end time 
    //============================
    public class MeetingHub : Hub
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDoctorAppointmentRepository _doctorAppointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public MeetingHub(IHttpContextAccessor httpAccessor, UserManager<AppUser> userManager,
            IDoctorAppointmentRepository doctorAppointmentRepository,
            IDoctorRepository doctorRepository)
        {
            _httpAccessor = httpAccessor;
            _userManager = userManager;
            _doctorAppointmentRepository = doctorAppointmentRepository;
            _doctorRepository = doctorRepository;
        }



        //============================
        // On Disconnect
        //============================

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            if (_httpAccessor.HttpContext.User.Identity.IsAuthenticated && (_httpAccessor.HttpContext.User.IsInRole("Member") || _httpAccessor.HttpContext.User.IsInRole("Doctor")))
            {

                var user = await _userManager.FindByNameAsync(_httpAccessor.HttpContext.User.Identity.Name);

                if (user != null)
                {
                    await Clients.All.SendAsync("user-disconnected", (user.PeerId));
                    await Clients.All.SendAsync("setAsOffline", user.Id);

                    user.ConnectionId = null;
                    user.PeerId = null;
                    var result = await _userManager.UpdateAsync(user);


                }
                await base.OnDisconnectedAsync(exception);
            }
        }


        //============================
        // On Connect
        //============================
        public async override Task OnConnectedAsync()
        {
            if (_httpAccessor.HttpContext.User.Identity.IsAuthenticated && (_httpAccessor.HttpContext.User.IsInRole("Member") || _httpAccessor.HttpContext.User.IsInRole("Doctor")))
            {

                var user = await _userManager.FindByNameAsync(_httpAccessor.HttpContext.User.Identity.Name);

                if (user != null)
                {
                    user.ConnectionId = Context.ConnectionId;
                    var result = await _userManager.UpdateAsync(user);

                    await Clients.All.SendAsync("setAsOnline", user.Id);
                }

            }
            await base.OnConnectedAsync();
        }


        //============================
        // Join Room
        //============================
        public async Task JoinRoom(string roomId, string peerId, string appointmentId)
        {
            if (_httpAccessor.HttpContext.User.Identity.IsAuthenticated && (_httpAccessor.HttpContext.User.IsInRole("Member") || _httpAccessor.HttpContext.User.IsInRole("Doctor")))
            {
                var user = await _userManager.FindByNameAsync(_httpAccessor.HttpContext.User.Identity.Name);

                if (user != null)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                    await Clients.Group(roomId).SendAsync("user-connected", peerId);
                    if (_httpAccessor.HttpContext.User.IsInRole("Doctor"))
                    {
                        var id = Int32.Parse(appointmentId);
                        var appointment = await _doctorAppointmentRepository.GetAsync(x => x.Id == id);
                        if (appointment != null && appointment.StartedAt == null)
                        {
                            appointment.StartedAt = DateTime.UtcNow.AddHours(4);
                            _doctorAppointmentRepository.Commit();
                        }
                    }
                }

            }


        }
        //============================
        // Set Peer Id
        //============================
        public async Task SetPeerId(string peerId)
        {
            if (_httpAccessor.HttpContext.User.Identity.IsAuthenticated && (_httpAccessor.HttpContext.User.IsInRole("Member") || _httpAccessor.HttpContext.User.IsInRole("Doctor")))
            {
                var user = await _userManager.FindByNameAsync(_httpAccessor.HttpContext.User.Identity.Name);

                if (user != null)
                {
                    user.PeerId = peerId;
                    var result = await _userManager.UpdateAsync(user);
                }

            }

        }

        //============================
        // Set End Time
        //============================
        public async Task SetEndTime(string appointmentId)
        {
            int id = Int32.Parse(appointmentId);
            var appointment = await _doctorAppointmentRepository.GetAsync(x=> x.Id == id, "Doctor");
            if(appointment != null)
            {
                var payment =  appointment.Doctor?.MeetingPrice ?? 0;
                appointment.FinishedAt = DateTime.UtcNow.AddHours(4);
                appointment.TotalPaid = payment / 10 *  (decimal)((TimeSpan)(appointment.FinishedAt - appointment.StartedAt)).TotalMinutes;
                if (appointment.TotalPaid < payment)
                    appointment.TotalPaid = payment;
                _doctorAppointmentRepository.Commit();
            }

        }

        //============================
        // RejectCall
        //============================
        public async Task RejectCall(int appointmentId)
        {
            var appointment = await _doctorAppointmentRepository.GetAsync(x => x.Id == appointmentId, "AppUser", "Doctor");
            if (appointment != null)
            {
                await Clients.Client(appointment.AppUser.ConnectionId).SendAsync("Rejected");
            }

        }

        //============================
        // RejectCall
        //============================
        public async Task CloseCall(string appointmentId)
        {
            var appointment = await _doctorAppointmentRepository.GetAsync(x => x.Id ==Int32.Parse(appointmentId) , "AppUser", "Doctor");
            var doctor = _doctorRepository.GetDoctor(appointment.DoctorId??0);
            
            if (appointment != null && doctor != null)
            {
                await Clients.Client(appointment.AppUser.ConnectionId).SendAsync("CallClosed");
                await Clients.Client(doctor.AppUser.ConnectionId).SendAsync("CallClosed");
            }
        }
    }
}
