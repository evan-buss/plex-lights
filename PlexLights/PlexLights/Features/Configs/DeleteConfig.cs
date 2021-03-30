using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlexLights.Entities;
using PlexLights.Infrastructure;

namespace PlexLights.Features.Configs
{
    public static class DeleteConfig
    {
        public class Request : IRequest
        {
            public int ConfigId { get; init; }
        }

        public class Handler : AsyncRequestHandler<Request>
        {
            private readonly Context _context;

            public Handler(Context context)
            {
                _context = context;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.Configs.Remove(new Config { Id = request.ConfigId});
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}