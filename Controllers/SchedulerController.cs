using DbQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MicroServices.Controllers
{
    [RoutePrefix("api/scheduler")]
    public class SchedulerController : ApiController
    {
        /// <summary>
        /// Scheduler操作
        /// </summary>
        /// <param name="action">start、stop</param>
        /// <returns></returns>
        [HttpPost, Route(""), ResponseType(typeof(string))]
        public IHttpActionResult Post(string action)
        {
            string msg = "Do Nothing ...";
            Scheduler scheduler = new Scheduler();

            switch (action)
            {
                case "start":
                    scheduler.Start();
                    msg = "Scheduler Is Working";
                    break;
                case "stop":
                    scheduler.Stop();
                    msg = "Scheduler Are Stoped";
                    break;
                default:
                    break;
            }

            return Ok(msg);
        }
    }
}
