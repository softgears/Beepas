using System;
using System.Linq;
using Beepas.Web.Classes.Enums;

namespace Beepas.Web.Classes.Entities
{
    /// <summary>
    /// Игрок 
    /// </summary>
    public partial class User
    {
        #region Данные личного кабинета

        /// <summary>
        /// Отображаемое имя игрока
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName()
        {
            if (LoginProvider != "bee")
            {
                return string.Format("{0} {1}.", FirstName, LastName[0]);
            }
            else
            {
                return Login;
            }
        }

        /// <summary>
        /// Возвращает позицию пользователя в рейтинге
        /// </summary>
        /// <returns></returns>
        public int GetRatingPosition()
        {
            return 1;
        }

        /// <summary>
        /// Возвращает количество золотых монет у пользователя
        /// </summary>
        /// <returns></returns>
        public decimal GetGoldCoins()
        {
            decimal result = 0;
            foreach (var payment in GoldCoinsPayments.Where(p => p.Completed))
            {
                var amount = payment.Amount;
                if (payment.Direction == (short)PaymentDirection.Outcome)
                {
                    amount *= -1;
                }
                result += amount;
            }
            return result;
        }

        /// <summary>
        /// Возвращает количество меда у пользователя
        /// </summary>
        /// <returns></returns>
        public decimal GetHoney()
        {
            decimal result = 0;
            if (Hives.Count > 0)
            {
                result += Hives.Sum(h => h.CurrentHoney);
            }
            if (Warehouses.Count > 0)
            {
                result += Warehouses.Sum(w => w.CurrentHoney);
            }
            if (Transports.Count > 0)
            {
                result += Transports.Sum(t => t.CurrentHoney);
            }
            return result;
        }

        /// <summary>
        /// возвращает количество золотых слитков у пользователя
        /// </summary>
        /// <returns></returns>
        public decimal GetGoldBars()
        {
            decimal result = 0;
            foreach (var payment in GoldBarsPayments.Where(p => p.Completed))
            {
                var amount = payment.Amount;
                if (payment.Direction == (short)PaymentDirection.Outcome)
                {
                    amount *= -1;
                }
                result += amount;
            }
            return result;
        }

        #endregion

        #region Данные по пасеке

        /// <summary>
        /// Возвращает количество ульев у пользователя
        /// </summary>
        /// <returns></returns>
        public int GetHivesCount()
        {
            return Hives.Count;
        }

        /// <summary>
        /// Возвращает общее количество меда в ульях пользователя
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalHivesHoney()
        {
            decimal result = 0;
            if (Hives.Count > 0)
            {
                result += Hives.Sum(h => h.CurrentHoney);
            }
            return result;
        }

        /// <summary>
        /// Возвращает общее количество свободного места в ульях
        /// </summary>
        /// <returns></returns>
        public decimal GetAvailableHivesSpace()
        {
            decimal result = 0;
            if (Hives.Count > 0)
            {
                result += Hives.Sum(h => h.HiveType.Capacity - h.CurrentHoney);
            }
            return result;
        }

        /// <summary>
        /// Возвращает суммарную производительность всех ульев пользователя
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalProductionRate()
        {
            decimal result = 0;
            if (Hives.Count > 0)
            {
                result += Hives.Sum(h => h.HiveType.ProductionRate);
            }
            return result;
        }

        #endregion

        #region Данные по складам

        /// <summary>
        /// Возвращает количество складов у пользователя
        /// </summary>
        /// <returns></returns>
        public int GetWarehousesCount()
        {
            return Warehouses.Count;
        }

        /// <summary>
        /// Возвращает общее количество меда, который хранится на складе
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalWarehousesHoney()
        {
            decimal result = 0;
            if (Warehouses.Count > 0)
            {
                result += Warehouses.Sum(w => w.CurrentHoney);
            }
            return result;
        }

        /// <summary>
        /// Возвращает общую вместимость складов
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalWarehousesSpace()
        {
            decimal result = 0;
            if (Warehouses.Count > 0)
            {
                result += Warehouses.Sum(w => w.WarehouseType.Capacity);
            }
            return result;
        }

        /// <summary>
        /// Возвращает количество свободного емста на всех складах
        /// </summary>
        /// <returns></returns>
        public decimal GetAvailableWarehousesSpace()
        {
            decimal result = 0;
            if (Warehouses.Count > 0)
            {
                result += Warehouses.Sum(w => w.WarehouseType.Capacity - w.CurrentHoney);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Возвращает количество транспортных средств
        /// </summary>
        /// <returns></returns>
        public int GetTransportsCount()
        {
            return Transports.Count;
        }

        /// <summary>
        /// Возвращает свободное место в транспортных средствах 
        /// </summary>
        /// <returns></returns>
        public decimal GetAvailableTransportsSpace()
        {
            decimal result = 0;
            if (Transports.Count > 0)
            {
                result += Transports.Sum(t => t.TransportType.Capacity - t.CurrentHoney);
            }
            return result;
        }

        /// <summary>
        /// Возвращает общее количество меда, которое можно погрузить на весь транспорт
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalTransportsSpace()
        {
            decimal result = 0;
            if (Transports.Count > 0)
            {
                result += Transports.Sum(t => t.TransportType.Capacity);
            }
            return result;
        }

        /// <summary>
        /// Возвращает количество меда, который находится в пути
        /// </summary>
        /// <returns></returns>
        public decimal GetTransportsInTrafficHoney()
        {
            decimal result = 0;
            if (Transports.Count > 0)
            {
                result += Transports.Sum(t => t.CurrentHoney);
            }
            return result;
        }

        /// <summary>
        /// Выполняет списание пользовательского баланса на определенное количество денег
        /// </summary>
        /// <param name="amount">Количество</param>
        /// <param name="description">Описание списания</param>
        public void ChargeGoldCoins(decimal amount, string description)
        {
            var newPayment = new GoldCoinsPayment()
            {
                User = this,
                Amount = amount,
                Completed = true,
                DateCompleted = DateTime.Now,
                DateCreated = DateTime.Now,
                Direction = (short)PaymentDirection.Outcome,
                Description = description
            };
            this.GoldCoinsPayments.Add(newPayment);
        }

        /// <summary>
        /// Пополняет баланс указанного пользователя на определенное количество денег
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="description"></param>
        public void AddGoldCoins(decimal amount, string description)
        {
            var newPayment = new GoldCoinsPayment()
                {
                    User = this,
                    Amount = amount,
                    Completed = true,
                    DateCompleted = DateTime.Now,
                    DateCreated = DateTime.Now,
                    Direction = (short) PaymentDirection.Income,
                    Description = description
                };
            this.GoldCoinsPayments.Add(newPayment);
        }
    }
}