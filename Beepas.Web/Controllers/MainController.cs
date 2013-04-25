using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beepas.Web.Classes.Managers;

namespace Beepas.Web.Controllers
{
    /// <summary>
    /// Основной контроллер системы
    /// </summary>
    public class MainController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
        /// </summary>
        public MainController(UsersManager usersManager) : base(usersManager)
        {
        }

        /// <summary>
        /// Главная страница
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отображает страницу не найдено
        /// </summary>
        /// <param name="aspxerrorpath">Путь, который не найден</param>
        /// <returns></returns>
        public ActionResult NotFound(string aspxerrorpath)
        {
            return View();
        }

    }
}
