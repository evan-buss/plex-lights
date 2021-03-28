using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlexLights.Entities;
using PlexLights.Infrastructure;
using PlexLights.Models;

namespace PlexLights.Repositories
{
    public class DeviceRepository
    {
        private readonly Context _context;

        public DeviceRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task AddDevice(string name, string clientId)
        {
            if (!await _context.Devices.AnyAsync(x => x.ClientId == clientId))
            {
                _context.Devices.Update(new Device
                {
                    Name = name,
                    ClientId = clientId
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}