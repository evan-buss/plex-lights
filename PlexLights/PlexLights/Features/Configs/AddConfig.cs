using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlexLights.Entities;
using PlexLights.Infrastructure;

namespace PlexLights.Features.Configs
{
    public static class AddConfig
    {
        public class Request : IRequest
        {
            public string Name { get; set; }
            public int DeviceId { get; set; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly Context _context;

            public Handler(Context context)
            {
                this._context = context;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Configs.Add(new Config
                {
                    Name = request.Name,
                    DeviceId = request.DeviceId,
                    IsActive = true
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}