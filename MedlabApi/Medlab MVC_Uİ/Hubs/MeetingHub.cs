using Medlab.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Medlab_MVC_Uİ.Hubs
{

    public class MeetingHub : Hub
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly UserManager<AppUser> _userManager;

        public MeetingHub(IHttpContextAccessor httpAccessor, UserManager<AppUser> userManager)
        {
            _httpAccessor = httpAccessor;
            _userManager = userManager;
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

                await Clients.All.SendAsync("setAsOnline", "This is user");

            }
            await base.OnConnectedAsync();
        }


        //============================
        // Join Room
        //============================
        public async Task JoinRoom(string roomId, string peerId)
        {
            if (_httpAccessor.HttpContext.User.Identity.IsAuthenticated && (_httpAccessor.HttpContext.User.IsInRole("Member") || _httpAccessor.HttpContext.User.IsInRole("Doctor")))
            {
                var user = await _userManager.FindByNameAsync(_httpAccessor.HttpContext.User.Identity.Name);

                if (user != null)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
                    await Clients.Group(roomId).SendAsync("user-connected", peerId);
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
        // Request Confirmaiton
        //============================
        public async Task SendRing(string confirmerId )
        {
            var callerId = Context.ConnectionId;
            // Send a confirmation request to the user
            await Clients.All.SendAsync("recieveSignal", (callerId));

        }

        //=======================
        // Reply Confirmation
        //=======================

        public async Task ConfirmCallRequest(string callerId, bool isConfirmed)
        {
            var confirmerId = Context.ConnectionId;

            await Clients.User(callerId).SendAsync("user-replied", isConfirmed, confirmerId);
        }


    }
}
