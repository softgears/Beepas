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
    /// Контроллер управления статистикой
    /// </summary>
    public class StatisticsController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
        /// </summary>
        public StatisticsController(UsersManager usersManager) : base(usersManager)
        {
        }

        /// <summary>
        /// Главная страница статистики, отображающая общую статистику игры
        /// </summary>
        /// <returns></returns>
        [Route("stats")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
