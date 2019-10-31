using Microsoft.AspNetCore.Mvc;

namespace NewWebAPP201910.Controllers
{
    public class MonitorController : Controller
    {
        public ActionResult Index(){
            return RedirectToAction("Index","Home");
        }

        public ActionResult ReceivedBytes()
        {
            return Content(NetworkMonitor.NetworkReceived() / 1000 + "KB/s");
        }

        public ActionResult SentBytes()
        {
            return Content(NetworkMonitor.NetworkSent() / 1000 + "KB/s");
        }

    }
}
