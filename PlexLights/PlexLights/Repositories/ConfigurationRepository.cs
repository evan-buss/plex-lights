using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PlexLights.Entities;
using PlexLights.Infrastructure;
using PlexLights.Models;

namespace PlexLights.Repositories
{
    public class ConfigurationRepository
    {
        private readonly Context _context;

        public ConfigurationRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Config>> GetConfigurations()
        {
            return await _context.Configs.Include(x => x.Device).Include(x => x.Lights).ToListAsync();
        }

        public async Task CreateConfiguration(CreateConfig config)
        {
            // ReSharper disable once MethodHasAsyncOverload
            _context.Configs.Add(new Config()
            {
                Name = config.Name,
                DeviceId = config.DeviceId,
                IsActive = true
            });

            await _context.SaveChangesAsync();
        }

        public Task<List<Config>> FindConfigsForClientId(string clientId)
        {
            return _context.Configs
                .Include(x => x.Device)
                .Where(x => x.Device.ClientId == clientId && x.IsActive)
                .ToListAsync();
        }
    }
}