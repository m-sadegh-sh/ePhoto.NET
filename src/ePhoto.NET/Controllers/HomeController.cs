using System.Web.Mvc;

namespace ePhoto.NET.Controllers {
    [HandleError(View = "Error")]
    public partial class HomeController : Controller {
        [HttpGet]
        public virtual ActionResult About() {
            return View(Views.About);
        }

        [HttpGet]
        public virtual ActionResult Contact() {
            return View(Views.Contact);
        }
    }
}