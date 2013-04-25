using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Beepas.Web.Classes.IoC;
using Beepas.Web.Classes.Managers;

namespace Beepas.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Запуск приложения
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            IoCConfig.Init();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Начала сессии пользователя
        /// </summary>
        protected void Session_Start()
        {
            using (var httpRequestScope = Locator.BeginNestedHttpRequestScope())
            {
                // Контекст
                var context = HttpContext.Current;

                // Ищем авторизационную куку
                var authCookie = context.Request.Cookies["auth"];
                if (authCookie != null)
                {
                    var identity = authCookie["identity"];
                    var pass = authCookie["pass"];

                    var manager = Locator.GetService<UsersManager>();
                    var user = manager.FindUserByLoginAndPasswordHash(identity, pass);
                    if (user != null)
                    {
                        context.Session["CurrentUser"] = user.Id;
                        manager.SubmitChanges();
                    }
                    else
                    {
                        context.Response.Cookies.Remove("auth");
                    }
                }
            }
        }
    }
}