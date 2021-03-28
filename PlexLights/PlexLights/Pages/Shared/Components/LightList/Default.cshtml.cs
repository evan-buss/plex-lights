using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlexLights.Models;
using PlexLights.Repositories;

namespace PlexLights.Pages.Shared.Components.LightList
{
    public class LightList : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}