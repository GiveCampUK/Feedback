using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SceneAndHeardFeedback.Models
{
    public class EventsWrapper
    {
        public List<EventWrapper> Events { get; set; }
    }

    public class EventWrapper
    {
        public Event Event { get; set; }
    }

    public class Event
    {
        public Int64 id { get; set; }
        public string Description { get; set; }
    }
}