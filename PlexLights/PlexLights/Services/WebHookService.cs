using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PlexLights.Entities;
using PlexLights.Repositories;

namespace PlexLights.Services
{
    public class WebHookService
    {
        private readonly DeviceRepository deviceRepository;
        private readonly ConfigurationRepository configRepository;
        private readonly IHubContext<ActivityHub, IActivityHub> hubContext;

        public WebHookService(IHubContext<ActivityHub, IActivityHub> hubContext, DeviceRepository deviceRepository, ConfigurationRepository configRepository)
        {
            this.deviceRepository = deviceRepository;
            this.configRepository = configRepository;
            this.hubContext = hubContext;
        }

        public async Task OnEvent(dynamic payload)
        {
            dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(payload, new ExpandoObjectConverter());
            await deviceRepository.AddDevice(json.Player.title, json.Player.uuid);
            List<Config> configs = await configRepository.FindConfigsForClientId(json.Player.uuid);

            Console.WriteLine(configs.Count);

            await hubContext.Clients.All.SendActivity();
        }
    }
}