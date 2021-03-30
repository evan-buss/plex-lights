using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlexLights.Entities;
using PlexLights.Infrastructure;

namespace PlexLights.Features.Lights
{
    public static class GetLights
    {
        public class Request : IRequest<List<Light>> { }

        public class Handler : IRequestHandler<Request, List<Light>>
        {
            private readonly Context _context;

            public Handler(Context context)
            {
                this._context = context;
            }

            public async Task<List<Light>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _context.Lights.ToListAsync(cancellationToken);
            }
        }
    }
}