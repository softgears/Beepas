using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beepas.Web.Classes.Managers;
using Beepas.Web.Classes.Routing;

namespace Beepas.Web.Controllers
{
    /// <summary>
    /// Контроллер управления чатом
    /// </summary>
    public class MessengerController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
        /// </summary>
        public MessengerController(UsersManager usersManager) : base(usersManager)
        {
        }

        /// <summary>
        /// Отображает чат
        /// </summary>
        /// <returns></returns>
        [Route("chat")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
