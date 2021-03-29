using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlexLights.Entities;
using PlexLights.Infrastructure;
using PlexLights.Models;

namespace PlexLights.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Context context;

        public IndexModel(ILogger<IndexModel> logger, Context context)
        {
            _logger = logger;
            this.context = context;
        }

        public IEnumerable<Light> Lights { get; set; }
        public IEnumerable<Device> Devices { get; set; }

        public async Task OnGetAsync()
        {
            Lights = await context.Lights.ToListAsync();
            Devices = await context.Devices.ToListAsync();
        }

        public async Task<IActionResult> OnPostAddLightAsync([FromForm] string lightName, [FromForm] string lightIp)
        {
            context.Lights.Add(new Light()
            {
                Name = lightName,
                IPAddress = lightIp
            });

            await context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostAddConfigAsync(CreateConfig config)
        {
            context.Configs.Add(new Config()
            {
                Name = config.Name,
                DeviceId = config.DeviceId,
                IsActive = true
            });

            await context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}