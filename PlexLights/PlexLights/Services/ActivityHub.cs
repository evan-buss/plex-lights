using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlexLights.Entities;

namespace PlexLights.Services
{
    public interface IActivityHub
    {
        Task SendActivity(History history);
    }

    public class ActivityHub : Hub<IActivityHub>
    {
        public async Task OnActivity(History history)
        {
            await Clients.All.SendActivity(history);
        }
    }
}