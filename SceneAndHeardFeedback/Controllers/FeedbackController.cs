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

        private FeedbackContext db = new FeedbackContext();
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

        public ActionResult LeaveFeedback(Int64? id)
        {
            var feedback = new Feedback();
            if (id.HasValue)
            {
                feedback.eventBriteId = id.Value.ToString();
                feedback.FeedbackLeft = DateTime.Today;
            }
            return View(feedback);
        }

        [HttpPost]
        public ActionResult LeaveFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedback.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Thanks", "Feedback");
            }
            else
            {
                return View(feedback);
            }
        }

        public ActionResult Thanks()
        {
            return View();
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

}
