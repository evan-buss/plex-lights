using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlexLights.Entities;
using PlexLights.Infrastructure;
using PlexLights.Models;

namespace PlexLights.Repositories
{
    public class LightRepository
    {
        private readonly Context _context;

        public LightRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Light>> GetAllLights()
        {
            return await _context.Lights.ToListAsync();
        }

        public async Task SaveLight(string lightName, string lightIp)
        {
            _context.Lights.Add(new Light()
            {
                Name = lightName,
                IPAddress = lightIp
            });

            await _context.SaveChangesAsync();
        }
    }
}