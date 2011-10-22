using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EasyHttp.Http;
using SceneAndHeardFeedback.Models;
using Util.ConfigManager;

namespace SceneAndHeardFeedback.Controllers
{
    public class FeedbackController : Controller
    {
        private EventBriteLayer _eventBriteApi;
        private IConfigManager _configManager = new ConfigManager();
        //
        // GET: /Feedback/

        public ActionResult Index()
        {
            _eventBriteApi = new EventBriteLayer();

            var events = _eventBriteApi.GetEvents(_configManager.GetAppSetting("EventBriteAPIKey"), 
                                                  _configManager.GetAppSetting("EventBriteUserKey"), 
                                                  _configManager.GetAppSettingAs<int>("EventBriteOrganiserId"));

            return View(events);
        }

        public ActionResult LeaveFeedback(int? id)
        {
            var feedback = new Feedback();
            if (id.HasValue)
            {
                feedback.eventBriteId = id.Value.ToString();
            }
            return View(feedback);
        }

        [HttpPost]
        public ActionResult LeaveFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
            }
            return RedirectToAction("Index", "Feedback");
        }

    }

    public class EventBriteLayer
    {
        public List<Event> GetEvents(string appKey, string userKey, int organiserId)
        {
            var eventWrappers = Get<EventsWrapper>(appKey, userKey, organiserId).Events;

            return eventWrappers.Select(eventWrapper => eventWrapper.Event).ToList();
        }

        private T Get<T>(string appKey, string userKey, int organiserId)
        {
            var http = new HttpClient
            {
                Request = { Accept = HttpContentTypes.ApplicationJson }
            };

            var url =
                string.Format("https://www.eventbrite.com/json/organizer_list_events?app_key={0}&user_key={1}&id={2}",
                              appKey, userKey, organiserId);

            try
            {
                var response = http.Get(url).StaticBody<T>();
                return response;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }

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
