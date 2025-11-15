using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;
using Microsoft.AspNetCore.SignalR;

namespace HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services
{
    public class RealtimeNotifier : IRealtimeNotifier
    {
        private readonly IHubContext<ReloadHub> _hubContext;

        public RealtimeNotifier(IHubContext<ReloadHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task NotifyReloadAsync(string key)
        {
            return _hubContext.Clients.All.SendAsync("ReloadData", key);
        }
    }
}
