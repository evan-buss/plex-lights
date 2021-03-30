using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlexLights.Entities;
using PlexLights.Models;
using PlexLights.ViewModels;

namespace PlexLights.Pages.Shared.Components.ConfigForm
{
    public class ConfigForm : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IndexViewModel model)
        {
            return View(model);
        }
    }
}