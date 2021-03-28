using Microsoft.EntityFrameworkCore;
using PlexLights.Entities;
using PlexLights.Models;

namespace PlexLights.Infrastructure
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Light> Lights { get; set; }
        public DbSet<Config> Configs { get; set; }
    }
}