using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PlexLights.Infrastructure;
using PlexLights.Services;

namespace PlexLights.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class WebHook : PageModel
    {
        private readonly Context _context;
        private readonly WebHookService webHookService;

        public WebHook(Context context, WebHookService webHookService)
        {
            _context = context;
            this.webHookService = webHookService;
        }

        public async Task OnGetAsync()
        {
            Items = await _context.Devices.Select(x => x.Name).WriteQueryString().ToListAsync();
        }

        public List<string> Items { get; set; }

        public async Task<IActionResult> OnPost([FromForm] string payload)
        {
            await webHookService.OnEvent(payload);
            return new OkResult();
        }
    }
}