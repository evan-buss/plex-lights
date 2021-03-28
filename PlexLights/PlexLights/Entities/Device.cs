using System.ComponentModel.DataAnnotations;

namespace PlexLights.Entities
{
    public record Device
    {
        public int Id { get; init; }
        [StringLength(128)] public string Name { get; init; }
        [StringLength(128)] public string ClientId { get; init; }
    }
}