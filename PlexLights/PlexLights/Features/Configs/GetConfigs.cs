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

namespace PlexLights.Features.Configs
{
    public static class GetConfigs
    {
        public class Request : IRequest<IEnumerable<Config>> { }

        public class Handler : IRequestHandler<Request, IEnumerable<Config>>
        {
            private readonly Context _context;

            public Handler(Context context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Config>> Handle(Request request, CancellationToken cancellationToken)
            {
                return await _context.Configs.ToListAsync(cancellationToken);
            }
        }
    }
}