using System.Collections.Generic;
using PlexLights.Entities;

namespace PlexLights.ViewModels
{
    public class IndexViewModel
    {
        public List<Light> Lights { get; set; }
        public List<Device> Devices { get; set; }
    }
}