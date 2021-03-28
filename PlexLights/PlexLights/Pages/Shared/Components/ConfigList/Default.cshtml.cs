using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlexLights.Models;
using PlexLights.Repositories;

namespace PlexLights.Pages.Shared.Components.ConfigList
{
    public class ConfigList : ViewComponent
    {
        private readonly ConfigurationRepository _repository;

        public ConfigList(ConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configs = await _repository.GetConfigurations();
            return View(configs.ToList());
        }
    }
}