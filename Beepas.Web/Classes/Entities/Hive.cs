using System;
using System.Linq;

namespace Beepas.Web.Classes.Entities
{
    /// <summary>
    /// Улей, производящий мед
    /// </summary>
    public partial class Hive
    {
        /// <summary>
        /// Процент заполненности улья
        /// </summary>
        public string FillPercentage
        {
            get { return String.Format("{0:0}%",Math.Round(CurrentHoney/HiveType.Capacity*100)); }
        }

        /// <summary>
        /// Улей заполнен полностью
        /// </summary>
        public bool IsFull
        {
            get { return CurrentHoney >= HiveType.Capacity; }
        }

        /// <summary>
        /// Собирает мед в указанном улье
        /// </summary>
        public void CollectHoney()
        {
            var honey = CurrentHoney;

            // Получаем список свободных складов
            var freeWarehouses = User.Warehouses.Where(w => w.GetFreeSpace() > 0).ToList();

            // Распределяем
            while (honey > 0 || freeWarehouses.Count > 0)
            {
                // Берем первый склад
                var warehouse = freeWarehouses.First();

                // Определяем сколько мы можем в него поместить
                var decrement = warehouse.GetFreeSpace();
                if (decrement > honey)
                {
                    decrement = honey;
                }

                // Уменьшаем количество меда
                honey -= decrement;
                warehouse.CurrentHoney += decrement;

                freeWarehouses.RemoveAt(0);
            }

            // Сохраняем
            CurrentHoney = honey;   
        }
    }
}