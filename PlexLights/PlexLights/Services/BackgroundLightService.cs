using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PlexLights.Infrastructure;
using PlexLights.Models;
using WiZ;

namespace PlexLights.Services
{
    public class BackgroundLightService : BackgroundService
    {

        public static ChannelWriter<MediaEvent> Writer { get { return _events.Writer; } }

        private static readonly Channel<MediaEvent> _events 
            = Channel.CreateUnbounded<MediaEvent>(new UnboundedChannelOptions { SingleReader = true });

        private readonly TimerCollection lightEvents = new();
        private readonly IServiceProvider serviceProvider;

        public BackgroundLightService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var item in _events.Reader.ReadAllAsync(stoppingToken))
            {
                lightEvents.AddOrUpdate(item.ClientId, () => DoThing(item), TimeSpan.FromSeconds(5));
            }
        }

        private void DoThing(MediaEvent item)
        {
            var bulb = new Bulb("192.168.1.2");
           
            if (item.Type == EventType.Play || item.Type == EventType.Resume)
            {
                bulb.Brightness = 10;
            }
            else if (item.Type == EventType.Pause || item.Type == EventType.Stop)
            {
                bulb.Brightness = 100;
            }
        }
    }
}