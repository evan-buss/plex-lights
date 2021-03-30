using Microsoft.AspNetCore.Mvc;

namespace PlexLights.Pages.Shared.Components.LightForm
{
    public class LightForm : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}