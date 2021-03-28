using System;
using System.ComponentModel.DataAnnotations;

namespace PlexLights.Models
{
    public class CreateConfig
    {
        [MinLength(4)] public string Name { get; set; }
        [Range(1, long.MaxValue)] public int LightId { get; set; }
        [Range(1, long.MaxValue)] public int DeviceId { get; set; }
    }
}