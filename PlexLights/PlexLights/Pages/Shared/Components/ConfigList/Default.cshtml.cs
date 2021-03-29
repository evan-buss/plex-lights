using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlexLights.Infrastructure;

namespace PlexLights.Pages.Shared.Components.ConfigList
{
    public class ConfigList : ViewComponent
    {
        private readonly Context _context;

        public ConfigList(Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configs = await _context.Configs.Include(x => x.Device).Include(x => x.Lights).ToListAsync();
            return View(configs.ToList());
        }
    }
}