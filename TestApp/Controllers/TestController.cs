using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestApp.Controllers
{
    public class TestController : Controller
    {
        private Random rnd = new Random();
        public ActionResult Job1()
        {
            System.Threading.Thread.Sleep(5000);
            return Content(Guid.NewGuid().ToString());
        }

        public ActionResult Job2()
        {
            System.Threading.Thread.Sleep(1000);

            if(rnd.Next(5) > 3)
                throw new Exception("FAIL");

            return Content("SUCCESS");
        }
    }
}
