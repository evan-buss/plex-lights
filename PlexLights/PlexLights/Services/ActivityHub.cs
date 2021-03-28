using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PlexLights.Services
{
    public interface IActivityHub
    {
        Task SendActivity();
    }

    public class ActivityHub : Hub<IActivityHub>
    {
        public async Task OnActivity()
        {
            await Clients.All.SendActivity();
        }
    }
}