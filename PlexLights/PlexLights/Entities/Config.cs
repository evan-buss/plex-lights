using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlexLights.Entities
{
    public record Config
    {
        public int Id { get; init; }
        [Required] [StringLength(128)] public string Name { get; init; }
        public bool IsActive { get; init; }
        [MinLength(1)] public List<Light> Lights { get; init; }
        public int DeviceId { get; set; }
        public Device Device { get; init; }
    }
}