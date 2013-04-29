using System;
using System.Linq;
using Beepas.Web.Classes.DAL;
using Beepas.Web.Classes.Enums;

namespace Beepas.Web.Classes.Managers
{
    /// <summary>
    /// Менеджер статистики игры
    /// </summary>
    public class StatsManager
    {
        /// <summary>
        /// Контекст доступа к данным
        /// </summary>
        public BeepasDataContext DataContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public StatsManager(BeepasDataContext dataContext)
        {
            DataContext = dataContext;
        }

        /// <summary>
        /// Возвращает общее количество игроков
        /// </summary>
        /// <returns></returns>
        public int GetTotalPlayers()
        {
            return DataContext.Users.Count();
        }

        /// <summary>
        /// Возвращает количество игроков зарегистрированных сегодня
        /// </summary>
        /// <returns></returns>
        public int GetNewPlayers()
        {
            return DataContext.Users.Count(u => u.DateRegistred.Value.Date == DateTime.Now.Date);
        }

        /// <summary>
        /// Возвращает размер банка игры
        /// </summary>
        /// <returns></returns>
        public decimal GetGameBank()
        {
            decimal result = 0;
            var incomePayments =
                DataContext.GoldCoinsPayments.Where(p => p.Direction == (short)PaymentDirection.Income).ToList();

            if (incomePayments.Count > 0)
            {
                result += incomePayments.Sum(p => p.Amount);
            }

            return result*(decimal) 0.6;
        }

        /// <summary>
        /// Возвращает общее количество ульев
        /// </summary>
        /// <returns></returns>
        public int GetTotalHives()
        {
            return DataContext.Hives.Count();
        }

        /// <summary>
        /// Возвращает общее количество собранного меда
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalHoneyCollected()
        {
            return DataContext.Users.Sum(u => u.TotalHoney);
        }

        /// <summary>
        /// Возвращает общее количество выплаченных выйгрышей
        /// </summary>
        /// <returns></returns>
        public decimal GetWinPayments()
        {
            decimal result = 0;
            var outPayments =
                DataContext.GoldCoinsPayments.Where(p => p.Direction == (short)PaymentDirection.Outcome).ToList();

            if (outPayments.Count > 0)
            {
                result += outPayments.Sum(p => p.Amount);
            }

            return result * (decimal)0.6;
        }
    }
}