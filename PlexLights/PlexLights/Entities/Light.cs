using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlexLights.Models;

namespace PlexLights.Entities
{
    public record Light
    {
        public int Id { get; init; }
        [StringLength(128)] public string Name { get; init; }
        [Required] public string IPAddress { get; init; }

        public List<Config> Configs { get; set; }
    }
}