using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SceneAndHeardFeedback.Models;
using EventbriteNET.Entities;
using EventbriteNET;

namespace SceneAndHeardFeedback.Controllers
{
    public class FeedbackController : Controller
    {
        private EventbriteNET.EventbriteContext context;
        //
        // GET: /Feedback/

        public ActionResult Index()
        {
            context = new EventbriteContext("27SKVYI2C5KFLVID6W", null);

            Organizer sceneandheard = new Organizer(1593730564, context);
            var eventList = sceneandheard.Events.Select(@event => @event.Value).ToList();


            return View(eventList);
        }

        public ActionResult LeaveFeedback()
        {
            return View();
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
}
