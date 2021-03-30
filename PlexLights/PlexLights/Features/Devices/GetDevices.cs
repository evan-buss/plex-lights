using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlexLights.Entities;
using PlexLights.Infrastructure;

namespace PlexLights.Features.Devices
{
    public class GetDevices
    {
        public class Request : IRequest<List<Device>>
        {
        }

        public class Handler : IRequestHandler<Request, List<Device>>
        {
            private readonly Context _context;

            public Handler(Context context)
            {
                _context = context;
            }

            public async Task<List<Device>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _context.Devices.ToListAsync(cancellationToken);
            }
        }
    }
}