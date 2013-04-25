using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beepas.Web.Classes.Entities;
using Beepas.Web.Classes.Enums;
using Beepas.Web.Classes.Managers;
using Beepas.Web.Classes.Routing;
using Beepas.Web.Classes.Utils;
using Beepas.Web.Models.Account;

namespace Beepas.Web.Controllers
{
    /// <summary>
    /// Контроллер управления аккаунтами пользователей
    /// </summary>
    public class AccountController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
        /// </summary>
        public AccountController(UsersManager usersManager) : base(usersManager)
        {

        }

        #region Авторизация

        /// <summary>
        /// Обрабатывает результат авторизации через соц сети
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost][Route("account/login-social")]
        public ActionResult LoginSocial(string token)
        {
            // Запрашиваем данные
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://ulogin.ru/token.php?token={0}&host={1}",token,"http://beepas.ru/account/login-social"));
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            string json = sr.ReadToEnd();
            sr.Close();
            response.Close();

            // Декодируем
            dynamic dataObject = new DynamicJsonObject(json);
            string network = dataObject.network;
            string identity = dataObject.identity;
            string firstName = dataObject.first_name;
            string lastName = dataObject.last_name;
            string photoUrl = dataObject.photo_big;
            string email = dataObject.email;

            // Ищем пользователя
            var user = UsersManager.FindUserByLogin(identity);
            if (user == null)
            {
                user = new User()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Login = identity,
                        PasswordHash = PasswordUtils.QuickMD5(PasswordUtils.GeneratePassword(10)),
                        AvatarUrl = photoUrl,
                        IsAdmin = false,
                        Status = (short) UserStatuses.Active,
                        AccountLevel = (short) AccountLevels.Common,
                        LoginProvider = network,
                        Email = email,
                        ReferalId = -1,
                        DateRegistred = DateTime.Now
                    };
                UsersManager.RegisterUser(user);
            }
            else
            {
                // Обновляем данные
                user.FirstName = firstName;
                user.LastName = lastName;
                user.AvatarUrl = photoUrl;
                user.DateModified = DateTime.Now;
                UsersManager.SubmitChanges();
            }

            // Авторизуем пользователя в системе
            AuthorizeUser(user,true);

            return RedirectToAction("Index", "Apiary");
        }

        /// <summary>
        /// Обрабатывает выход из системы
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            if (IsAuthentificated)
            {
                CloseAuthorization();    
            }
            return RedirectToAction("Index","Main");

        }

        /// <summary>
        /// Обрабатывает вход с использоанием логина и пароля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            // Ищем пользователя
            var user = UsersManager.FindUserByLoginAndPasswordHash(model.Login, PasswordUtils.QuickMD5(model.Password));
            if (user == null)
            {
                return View("LoginFailed");
            }

            // Авторизуем пользователя
            AuthorizeUser(user,model.RememberMe);

            // перенаправляемся на пасеку
            return RedirectToAction("Index", "Apiary");
        }

        #endregion

        #region Регистрация

        /// <summary>
        /// Проверяет наличие зарегистрированного логина в системе
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <returns></returns>
        [Route("account/check-login")]
        public ActionResult CheckLogin(string Login)
        {
            var exists = UsersManager.FindUserByLogin(Login) != null;
            if (exists)
            {
                return Content("Пользователь с таким логином уже зарегистрирован");
            }
            return Content("true");
        }

        /// <summary>
        /// Отображает страницу с формой регистрации
        /// </summary>
        /// <returns></returns>
        public ActionResult Register(long? id)
        {
            if (id.HasValue)
            {
                ViewBag.referrer = UsersManager.LoadUserById(id.Value);
            }
            return View();
        }

        /// <summary>
        /// Обрабатывает регистрацию пользователя
        /// </summary>
        /// <param name="model">Модель данных пользователя</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegistrationModel model)
        {
            // Проверяем уникальность логина
            var exist = UsersManager.FindUserByLogin(model.Login) != null;
            string message = "";
            if (!exist)
            {
                var user = new User()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Login = model.Login,
                        PasswordHash = PasswordUtils.QuickMD5(model.Password),
                        AvatarUrl = "/Content/images/layout/tmpavatar.png",
                        IsAdmin = false,
                        Status = (short) UserStatuses.Active,
                        AccountLevel = (short) AccountLevels.Common,
                        LoginProvider = "bee",
                        Email = model.Email,
                        ReferalId = model.ReffererId.HasValue ? model.ReffererId.Value : -1,
                        DateRegistred = DateTime.Now
                    };
                UsersManager.RegisterUser(user);

                message = "Вы были успешно зарегистрированы в системе";
                AuthorizeUser(user,true);
            }
            else
            {
                message = "Пользователь с таким логином уже существует";
            }

            ViewBag.message = message;
            return View("RegistrationResult");
        }

        #endregion

        #region Пасека

        /// <summary>
        /// Отображает страницу с производством
        /// </summary>
        /// <returns></returns>
        public ActionResult Apiary()
        {
            return RedirectToAction("Index", "Apiary");
        }

        #endregion

        #region Магазин

        /// <summary>
        /// Отображает страницу с магазином
        /// </summary>
        /// <returns></returns>
        public ActionResult Shop()
        {
            return View();
        }

        /// <summary>
        /// Выполняет покупку улья
        /// </summary>
        /// <returns>Покупка улья</returns>
        [Route("account/shop/buy-hive")]
        public ActionResult BuyHive()
        {
            // Проверяем что пользователь авторизован
            if (!IsAuthentificated)
            {
                return RedirectToAction("Register");
            }

            // Проверяем что на счету пользователя есть указанная сумма
            if (CurrentUser.GetGoldCoins() < 3000)
            {
                return View("NotEnoughMoney");
            }

            // Выполняем списание и добавляем новый улей
            CurrentUser.ChargeGoldCoins(3000, "Покупка нового улья");

            // Доабвляем пользователю новый склад 
            var newHive = new Hive()
                {
                    Status = 1,
                    CurrentHoney = 0,
                    DateCreated = DateTime.Now,
                    HasAutoCollector = false,
                    HiveTypeId = 1,
                    User = CurrentUser,
                    Notes = "Улей"
                };
            CurrentUser.Hives.Add(newHive);
            UsersManager.SubmitChanges();

            // перенаправляем на список ульев
            return RedirectToAction("Hives", "Apiary");
        }

        /// <summary>
        /// Выполняет покупку улья
        /// </summary>
        /// <returns>Покупка улья</returns>
        [Route("account/shop/buy-warehouse")]
        public ActionResult BuyWarehouse()
        {
            // Проверяем что пользователь авторизован
            if (!IsAuthentificated)
            {
                return RedirectToAction("Register");
            }

            // Проверяем что на счету пользователя есть указанная сумма
            if (CurrentUser.GetGoldCoins() < 500)
            {
                return View("NotEnoughMoney");
            }

            // Выполняем списание и добавляем новый склад
            CurrentUser.ChargeGoldCoins(500, "Покупка нового склада");

            // Доабвляем пользователю новый склад 
            var newWarehouse = new Warehouse()
                {
                    Status = 1,
                    CurrentHoney = 0,
                    DateCreated = DateTime.Now,
                    User = CurrentUser,
                    WarehouseTypeId = 1,
                    Notes = "Склад"
                };
            CurrentUser.Warehouses.Add(newWarehouse);
            UsersManager.SubmitChanges();

            // перенаправляем на список ульев
            return RedirectToAction("Warehouses", "Apiary");
        }

        /// <summary>
        /// Выполняет покупку транспорта
        /// </summary>
        /// <returns>Покупка улья</returns>
        [Route("account/shop/buy-transport")]
        public ActionResult BuyTransport()
        {
            // Проверяем что пользователь авторизован
            if (!IsAuthentificated)
            {
                return RedirectToAction("Register");
            }

            // Проверяем что на счету пользователя есть указанная сумма
            if (CurrentUser.GetGoldCoins() < 500)
            {
                return View("NotEnoughMoney");
            }

            // Выполняем списание и добавляем новый транспорт
            CurrentUser.ChargeGoldCoins(500, "Покупка нового транспорта");

            // Доабвляем пользователю новый склад 
            var transport = new Transport()
                {
                    Status = 1,
                    CurrentHoney = 0,
                    DateCreated = DateTime.Now,
                    User = CurrentUser,
                    TransportTypeId = 1,
                    Notes = "Транспорт"
                };
            CurrentUser.Transports.Add(transport);
            UsersManager.SubmitChanges();

            // перенаправляем на список ульев
            return RedirectToAction("Transport", "Apiary");
        }

        #endregion

        #region Пополнение баланса

        /// <summary>
        /// Отображает страницу пополнения баланса
        /// </summary>
        /// <returns></returns>
        [Route("account/add-balance")]
        public ActionResult AddBalance()
        {
            return View();
        }

        /// <summary>
        /// Обрабатывает пополнение баланса пользователя на 10000 монет
        /// </summary>
        /// <returns></returns>
        [Route("account/add-balance/debug")]
        public ActionResult AddBalanceDebug()
        {
            if (!IsAuthentificated)
            {
                return RedirectToAction("Register");
            }

            CurrentUser.AddGoldCoins(10000, "Пополнение счета через тестовое пополнение счета");
            UsersManager.SubmitChanges();

            return RedirectToAction("AddBalance");
        }

        #endregion
    }
}
