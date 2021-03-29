using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PlexLights.Entities;
using PlexLights.Infrastructure;
using PlexLights.Models;
using PlexLights.Services;

namespace PlexLights.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class WebHook : PageModel
    {
        private readonly Context _context;
        private readonly IHubContext<ActivityHub, IActivityHub> hub;

        public WebHook(Context context, IHubContext<ActivityHub, IActivityHub> hub)
        {
            _context = context;
            this.hub = hub;
        }

        public async Task OnGetAsync()
        {
            Items = await _context.Audit.Include(x => x.Device).OrderByDescending(x => x.Id).ToListAsync();
        }

        public List<History> Items { get; set; }

        public async Task<IActionResult> OnPost([FromForm] string payload)
        {
            dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(payload, new ExpandoObjectConverter());

            var @event = new MediaEvent
            {
                Title = json.Player.title,
                ClientId = json.Player.uuid,
                Type = json.@event switch
                {
                    "media.play" => EventType.Play,
                    "media.pause" => EventType.Pause,
                    "media.stop" => EventType.Stop,
                    "media.resume" => EventType.Resume,
                    _ => EventType.Other
                }
            };

            var device = await _context.Devices.SingleOrDefaultAsync(x => x.ClientId == @event.ClientId);

            // Save device
            if (device is null)
            {
                device = new Device
                {
                    Name = @event.Title,
                    ClientId = @event.ClientId,
                };
                _context.Devices.Update(device);
                await _context.SaveChangesAsync();
            }

            var historyItem = new History
            {
                DeviceId = device.Id,
                EventType = @event.Type,
                Title = json.Metadata.title,
            };

            _context.Audit.Add(historyItem);
            await _context.SaveChangesAsync();

            // Publish log for any listeners
            await hub.Clients.All.SendActivity(historyItem);

            // Queue event to trigger lighting.
            await BackgroundLightService.Writer.WriteAsync(@event);

            return new OkResult();
        }
    }
}