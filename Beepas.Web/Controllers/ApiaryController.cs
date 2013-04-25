using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beepas.Web.Classes.Entities;
using Beepas.Web.Classes.Managers;
using Beepas.Web.Classes.Routing;

namespace Beepas.Web.Controllers
{
    /// <summary>
    /// Контроллер управления пасекой
    /// </summary>
    public class ApiaryController : BaseController
    {
        /// <summary>
        /// Менеджер обновлений
        /// </summary>
        public UpgradesManager UpgradesManager { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.Controller"/> class.
        /// </summary>
        public ApiaryController(UsersManager usersManager, UpgradesManager upgradesManager) : base(usersManager)
        {
            UpgradesManager = upgradesManager;
        }

        /// <summary>
        /// Отображает главную страницу
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region Пасека

        /// <summary>
        /// Отображает список ульев
        /// </summary>
        /// <returns></returns>
        public ActionResult Hives()
        {
            return View("Hives",CurrentUser.Hives.ToList());
        }

        /// <summary>
        /// Модернизирует указанный улей
        /// </summary>
        /// <param name="id">Идентификатор улья</param>
        /// <returns></returns>
        [Route("apiary/hives/upgrade/{id}")]
        public ActionResult UpgradeHive(long id)
        {
            // Проверяем что собирается мед из нашего улья
            if (!IsAuthentificated || CurrentUser.Hives.All(h => h.Id != id))
            {
                throw new Exception("Ошибка доступа");
            }

            // Улей
            var hive = CurrentUser.Hives.FirstOrDefault(h => h.Id == id);
            if (hive == null)
            {
                return RedirectToAction("Hives");
            }

            // Узенаем, какой будет следующий апгрейд
            var nextLevel = UpgradesManager.GetNextHiveUpgrade(hive);
            if (nextLevel != null)
            {
                // Проверяем платежеспособность
                if (CurrentUser.GetGoldCoins() >= nextLevel.Price)
                {
                    // Обновляем улей
                    CurrentUser.ChargeGoldCoins(nextLevel.Price, string.Format("Обновление улья до уровня {0}", nextLevel.Level));
                    hive.HasAutoCollector = false; // Убираем автосборщик
                    hive.HiveType = nextLevel;
                    hive.DateModified = DateTime.Now;
                    UsersManager.SubmitChanges();
                }
                else
                {
                    return View("NotEnoughMoney");
                }
            }

            // Переходим к обновленному улью
            return Redirect("/apiary/hives/#" + id);
        }

        /// <summary>
        /// Собирает мед из указанного улья и пытается поместить его в склады
        /// </summary>
        /// <param name="id">Идентификатор улья</param>
        /// <returns></returns>
        [Route("apiary/hives/collect/{id}")]
        public ActionResult CollectHiveHoney(long id)
        {
            // Проверяем что собирается мед из нашего улья
            if (!IsAuthentificated || CurrentUser.Hives.All(h => h.Id != id))
            {
                throw new Exception("Ошибка доступа");
            }

            // Улей
            var hive = CurrentUser.Hives.FirstOrDefault(h => h.Id == id);
            hive.CollectHoney();
            UsersManager.SubmitChanges();

            // Переходим на список ульев
            return Redirect("/apiary/hives/#"+id);

        }

        /// <summary>
        /// Отображает страницу с активацией автоматического сборщика для указанного улья
        /// </summary>
        /// <param name="id">Идентификатор улья</param>
        /// <returns></returns>
        [Route("apiary/hives/choose-auto-collector/{id}")]
        public ActionResult ChooseAutoCollector(long id)
        {
            // Проверяем авторизованность
            if (CurrentUser == null)
            {
                return RedirectToAction("Register", "Account");
            }
            
            // Улей
            var hive = CurrentUser.Hives.FirstOrDefault(h => h.Id == id);
            if (hive == null)
            {
                return RedirectToAction("Hives");
            }

            ViewBag.autoCollectorType = UpgradesManager.GetAutoCollectorForHive(hive);
            return View(hive);
        }

        /// <summary>
        /// Обрабатывает включение или продление автоматического сборщика
        /// </summary>
        /// <param name="id">Идентификатор улья</param>
        /// <param name="period">КОличество дней</param>
        /// <returns></returns>
        [Route("apiary/hives/buy-auto-collector/{id}")]
        public ActionResult BuyAutoCollector(long id, int period)
        {
            // Проверяем авторизованность
            if (CurrentUser == null)
            {
                return RedirectToAction("Register", "Account");
            }

            // Улей
            var hive = CurrentUser.Hives.FirstOrDefault(h => h.Id == id);
            if (hive == null)
            {
                return RedirectToAction("Hives");
            }

            decimal price;
            DateTime endPeriod;
            var autoCollectorType = UpgradesManager.GetAutoCollectorForHive(hive);
            switch (period)
            {
                case 7:
                    endPeriod = hive.HasAutoCollector && hive.AutoCollectors != null &&
                                hive.AutoCollectors.Expiration.HasValue
                                    ? hive.AutoCollectors.Expiration.Value.AddDays(7)
                                    : DateTime.Now.AddDays(7);
                    price = autoCollectorType.WeekPrice;
                    break;
                case 31:
                    endPeriod = hive.HasAutoCollector && hive.AutoCollectors != null &&
                                hive.AutoCollectors.Expiration.HasValue
                                    ? hive.AutoCollectors.Expiration.Value.AddDays(31)
                                    : DateTime.Now.AddDays(31);
                    price = autoCollectorType.MonthPrice;
                    break;
                default:
                    endPeriod = hive.HasAutoCollector && hive.AutoCollectors != null &&
                                hive.AutoCollectors.Expiration.HasValue
                                    ? hive.AutoCollectors.Expiration.Value.AddDays(1)
                                    : DateTime.Now.AddDays(1);
                    price = autoCollectorType.DayPrice;
                    period = 1;
                    break;
            }

            // Проверяем наличие денег на счету пользователя
            if (!(CurrentUser.GetGoldCoins() >= price))
            {
                return View("NotEnoughMoney");
            }
            else
            {
                // Списываем деньги
                CurrentUser.ChargeGoldCoins(price,string.Format("Покупка автоматического сборщика на {0} дней", period));

                // Покупаем автоматический сборщик
                if (hive.AutoCollectors == null)
                {
                    hive.AutoCollectors = new AutoCollector()
                        {
                            AutoCollectorTypeId = hive.HiveType.Level,
                            Hive    = hive
                        };
                }
                hive.AutoCollectors.DateBuyed = DateTime.Now;
                hive.AutoCollectors.Expiration = endPeriod;
                hive.HasAutoCollector = true;
                UsersManager.SubmitChanges();
                return Redirect("/apiary/hives/#" + id);
            }
        }

        #endregion

        #region Склады

        /// <summary>
        /// Отображает список складов
        /// </summary>
        /// <returns></returns>
        public ActionResult Warehouses()
        {
            return View("Warehouses",CurrentUser.Warehouses.ToList());
        }

        /// <summary>
        /// Модернизирует указанный склад
        /// </summary>
        /// <param name="id">Идентификатор улья</param>
        /// <returns></returns>
        [Route("apiary/warehouses/upgrade/{id}")]
        public ActionResult UpgradeWarehouse(long id)
        {
            // Проверяем что обновляется наш склад
            if (!IsAuthentificated || CurrentUser.Warehouses.All(h => h.Id != id))
            {
                throw new Exception("Ошибка доступа");
            }

            // Улей
            var warehouse = CurrentUser.Warehouses.FirstOrDefault(h => h.Id == id);
            if (warehouse == null)
            {
                return RedirectToAction("Warehouses");
            }

            // Узенаем, какой будет следующий апгрейд
            var nextLevel = UpgradesManager.GetNextWarehouseUpgrade(warehouse);
            if (nextLevel != null)
            {
                // Проверяем платежеспособность
                if (CurrentUser.GetGoldCoins() >= nextLevel.Price)
                {
                    // Обновляем
                    CurrentUser.ChargeGoldCoins(nextLevel.Price, string.Format("Обновление склада до уровня {0}", nextLevel.Level));
                    warehouse.WarehouseType = nextLevel;
                    warehouse.DateModified = DateTime.Now;
                    UsersManager.SubmitChanges();
                }
                else
                {
                    return View("NotEnoughMoney");
                }
            }

            // Переходим к обновленному улью
            return Redirect("/apiary/warehouses/#" + id);
        }

        #endregion

        #region Транспортные средства

        /// <summary>
        /// Отображает список транспортных средств
        /// </summary>
        /// <returns></returns>
        public ActionResult Transport()
        {
            return View("Transport",CurrentUser.Transports.ToList());
        }

        /// <summary>
        /// Модернизирует указанный улей
        /// </summary>
        /// <param name="id">Идентификатор улья</param>
        /// <returns></returns>
        [Route("apiary/transport/upgrade/{id}")]
        public ActionResult UpgradeTransport(long id)
        {
            // Проверяем что обновляется наш транспорт
            if (!IsAuthentificated || CurrentUser.Transports.All(h => h.Id != id))
            {
                throw new Exception("Ошибка доступа");
            }

            // Улей
            var transport = CurrentUser.Transports.FirstOrDefault(h => h.Id == id);
            if (transport == null)
            {
                return RedirectToAction("Transport");
            }

            // Узенаем, какой будет следующий апгрейд
            var nextLevel = UpgradesManager.GetNextTransportUpgrade(transport);
            if (nextLevel != null)
            {
                // Проверяем платежеспособность
                if (CurrentUser.GetGoldCoins() >= nextLevel.Price)
                {
                    // Обновляем улей
                    CurrentUser.ChargeGoldCoins(nextLevel.Price, string.Format("Обновление транспорта до уровня {0}", nextLevel.Level));
                    transport.TransportType = nextLevel;
                    transport.DateModified = DateTime.Now;
                    UsersManager.SubmitChanges();
                }
                else
                {
                    return View("NotEnoughMoney");
                }
            }

            // Переходим к обновленному улью
            return Redirect("/apiary/transport/#" + id);
        }

        #endregion
    }
}
