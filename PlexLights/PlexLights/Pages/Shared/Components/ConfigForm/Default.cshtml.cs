using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlexLights.Entities;
using PlexLights.Models;

namespace PlexLights.Pages.Shared.Components.ConfigForm
{
    public class ConfigForm : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Device> devices, IEnumerable<Light> lights)
        {
            var model = new ConfigFormViewModel
            {
                Devices = devices.ToList(),
                Lights = lights.ToList()
            };
            return View(model);
        }
    }
}