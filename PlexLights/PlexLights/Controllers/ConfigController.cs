using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlexLights.Features.Configs;
using PlexLights.Features.Devices;
using PlexLights.Features.Lights;
using PlexLights.ViewModels;

namespace PlexLights.Controllers
{
    [ApiController]
    [Route("/api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConfigController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IndexViewModel>> Get()
        {
            return new IndexViewModel()
            {
                Devices = await _mediator.Send(new GetDevices.Request()),
                Lights = await _mediator.Send(new GetLights.Request()),
            };
        }

        [HttpPost]
        public async Task<ActionResult> AddLightAsync([FromBody] AddLight.Request request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AddConfigAsync([FromBody] AddConfig.Request request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{configId:int:min(1)}")]
        public async Task<ActionResult> DeleteConfigAsync([FromRoute] int configId)
        {
            await _mediator.Send(new DeleteConfig.Request {ConfigId = configId});
            return NoContent();
        }
    }
}