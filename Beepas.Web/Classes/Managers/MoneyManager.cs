using System.Linq;
using Beepas.Web.Classes.DAL;
using Beepas.Web.Classes.Enums;

namespace Beepas.Web.Classes.Managers
{
    /// <summary>
    /// Менеджер управления деньгами
    /// </summary>
    public class MoneyManager
    {
        /// <summary>
        /// Контекст доступа к данным
        /// </summary>
        public BeepasDataContext DataContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MoneyManager(BeepasDataContext dataContext)
        {
            DataContext = dataContext;
        }

        /// <summary>
        /// Возвращает текущий банк игры
        /// </summary>
        /// <returns></returns>
        public decimal GetGameBank()
        {
            decimal result = 0;
            var incomePayments =
                DataContext.GoldCoinsPayments.Where(p => p.Direction == (short) PaymentDirection.Income).ToList();

            if (incomePayments.Count > 0)
            {
                result += incomePayments.Sum(p => p.Amount);
            }

            return result;
        }
    }
}