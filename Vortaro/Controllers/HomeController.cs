using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Vortaro.Controllers.DAL;
using VortaroModel;

namespace Vortaro.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewData["Message"] = "欢迎使用 我的项目字典";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
