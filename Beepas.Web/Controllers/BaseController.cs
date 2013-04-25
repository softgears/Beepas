using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beepas.Web.Classes.Entities;
using Beepas.Web.Classes.IoC;
using Beepas.Web.Classes.Managers;

namespace Beepas.Web.Controllers
{
    /// <summary>
    /// Базовый контроллер системы
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public UsersManager UsersManager { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
        /// </summary>
        protected BaseController(UsersManager usersManager)
        {
            UsersManager = usersManager;
        }

        #region Сессии пользователей

        /// <summary>
        /// Хранение текущего пользователя
        /// </summary>
        private User _user { get; set; }

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public User CurrentUser
        {
            get
            {
                object fromSess = Session["CurrentUser"];
                if (fromSess == null)
                {
                    return null;
                }
                var userId = (long)fromSess;
                if (_user == null)
                {
                    _user = UsersManager.LoadUserById(userId);
                }
                return _user;
            }
            set
            {
                Session["CurrentUser"] = value != null ? (object)value.Id : null;
                if (value == null)
                {
                    Session.Remove("CurrentUser");
                }
                _user = value;
            }
        }

        /// <summary>
        /// Является ли текущий пользователь авторизованным
        /// </summary>
        public bool IsAuthentificated
        {
            get { return CurrentUser != null; }
        }

        /// <summary>
        /// Авторизирует текущего пользователя
        /// </summary>
        /// <param name="user">Пользователь которого установить как текущего</param>
        /// <param name="remember">Запомнить ли пользователя</param>
        public void AuthorizeUser(User user, bool remember = true)
        {
            CurrentUser = user;
            if (remember)
            {
                // Устанавливаем собственные авторизационные куки
                var authCookie = new HttpCookie("auth");
                authCookie.Values["identity"] = user.Login;
                authCookie.Values["pass"] = user.PasswordHash;
                authCookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(authCookie);
            }

            // Добавляем запись в таблицу авторизаций
            var loginHistoryItem = new UserLoginHistory()
                {
                    DateCreated = DateTime.Now,
                    User = user,
                    LoginDateTime = DateTime.Now,
                    LoginIP = Request.UserHostAddress
                };
            user.UserLoginHistories.Add(loginHistoryItem);
            UsersManager.SubmitChanges();
        }

        /// <summary>
        /// Убирает авторизацию текущего пользователя и убирает авторизационные куки если они есть
        /// </summary>
        public void CloseAuthorization()
        {
            CurrentUser = null;

            // убираем куки если они есть
            var authCookie = Response.Cookies["auth"];
            if (authCookie != null)
            {
                authCookie = new HttpCookie("auth")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(authCookie);
            }
        }

        #endregion
    }
}
