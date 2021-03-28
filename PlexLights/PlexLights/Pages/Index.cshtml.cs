using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PlexLights.Entities;
using PlexLights.Models;
using PlexLights.Repositories;

namespace PlexLights.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ConfigurationRepository _configRepository;
        private readonly LightRepository _lightRepository;
        private readonly DeviceRepository _deviceRepository;

        public IndexModel(ILogger<IndexModel> logger, ConfigurationRepository configRepository,
            LightRepository lightRepository, DeviceRepository deviceRepository)
        {
            _logger = logger;
            _configRepository = configRepository;
            _lightRepository = lightRepository;
            _deviceRepository = deviceRepository;
        }

        public IEnumerable<Light> Lights { get; set; }
        public IEnumerable<Device> Devices { get; set; }

        public async Task OnGetAsync()
        {
            Lights = await _lightRepository.GetAllLights();
            Devices = await _deviceRepository.GetAllDevices();
        }

        public async Task<IActionResult> OnPostAddLightAsync([FromForm] string lightName, [FromForm] string lightIp)
        {
            await _lightRepository.SaveLight(lightName, lightIp);
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostAddConfigAsync(CreateConfig config)
        {
            await _configRepository.CreateConfiguration(config);
            return RedirectToPage("Index");
        }
    }
}