using System;
using System.Web;
using System.Web.Mvc;
using SceneAndHeardFeedback.Models;
using Util.ConfigManager;

namespace SceneAndHeardFeedback.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly EventBriteLayer _eventBriteApi;
        private readonly IConfigManager _configManager;

        public FeedbackController()
        {
            _eventBriteApi = new EventBriteLayer();
            _configManager = new ConfigManager();
        }

        public ActionResult Index()
        {
            var events = _eventBriteApi.GetEvents(_configManager.GetAppSetting("EventBriteAPIKey"), 
                                                  _configManager.GetAppSetting("EventBriteUserKey"), 
                                                  _configManager.GetAppSettingAs<int>("EventBriteOrganiserId"));

            return View(events);
        }

        public ActionResult LeaveFeedback(Int64? id)
        {
            var feedback = new Feedback();
            if (id.HasValue)
                feedback.eventBriteId = id.Value.ToString();

            return View(feedback);
        }

        [HttpPost]
        public ActionResult LeaveFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                // we need to call a new layer here that will now work with the database

                //if this is correct then RedirectToAction - ThankYou
                return RedirectToAction("ThankYou");
            }

            return RedirectToAction("Index", "Feedback");
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}
