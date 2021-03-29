using System;
using System.ComponentModel.DataAnnotations;
using PlexLights.Models;

namespace PlexLights.Entities
{
    public class History
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }

        [StringLength(256)]
        public string Title { get; set; }
        public EventType EventType { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    }
}