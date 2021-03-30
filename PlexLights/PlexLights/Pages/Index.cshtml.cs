using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PlexLights.Features.Configs;
using PlexLights.Features.Devices;
using PlexLights.Features.Lights;
using PlexLights.Infrastructure;
using PlexLights.ViewModels;

namespace PlexLights.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(ILogger<IndexModel> logger, Context context, IMediator mediator)
        {
            _mediator = mediator;
        }

        public IndexViewModel ViewModel { get; } = new();

        public async Task OnGetAsync()
        {
            ViewModel.Lights = await _mediator.Send(new GetLights.Request());
            ViewModel.Devices = await _mediator.Send(new GetDevices.Request());
        }

        public async Task<IActionResult> OnPostAddLightAsync([FromForm] string lightName, [FromForm] string lightIp)
        {
            await _mediator.Send(new AddLight.Request {Name = lightName, IP = lightIp});
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostAddConfigAsync([FromForm] AddConfig.Request request)
        {
            await _mediator.Send(request);
            return RedirectToPage("Index");
        }
    }
}