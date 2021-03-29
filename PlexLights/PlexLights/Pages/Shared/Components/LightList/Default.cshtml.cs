using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlexLights.Models;

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