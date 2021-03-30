using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlexLights.Entities;
using PlexLights.Infrastructure;

namespace PlexLights.Features.Lights
{
    public static class AddLight
    {
        public class Request : IRequest
        {
            public string Name { get; set; }
            public string IP { get; set; }
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
                _context.Lights.Add(new Light()
                {
                    Name = request.Name,
                    IPAddress = request.IP
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}