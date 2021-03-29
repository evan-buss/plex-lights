using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlexLights.Models
{
    public class MediaEvent
    {
        public EventType Type { get; set; }
        public string Title { get; set; }
        public string ClientId { get; set; }
    }
}