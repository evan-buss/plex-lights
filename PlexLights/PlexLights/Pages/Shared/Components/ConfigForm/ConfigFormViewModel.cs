using System.Collections;
using System.Collections.Generic;
using PlexLights.Entities;
using PlexLights.Models;

namespace PlexLights.Pages.Shared.Components.ConfigForm
{
    public class ConfigFormViewModel
    {
        public List<Device> Devices { get; set; }
        public List<Light> Lights { get; set; }
    }
}