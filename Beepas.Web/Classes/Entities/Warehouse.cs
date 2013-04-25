using System;

namespace Beepas.Web.Classes.Entities
{
    /// <summary>
    /// Склад
    /// </summary>
    public partial class Warehouse
    {
        /// <summary>
        /// Количество доступного места на складе
        /// </summary>
        /// <returns></returns>
        public decimal GetFreeSpace()
        {
            return WarehouseType.Capacity - CurrentHoney;
        }

        /// <summary>
        /// Склад заполнен
        /// </summary>
        public bool IsFull
        {
            get { return CurrentHoney >= WarehouseType.Capacity; }
        }

        /// <summary>
        /// Процент заполненности склада
        /// </summary>
        public string FillPercentage
        {
            get { return String.Format("{0:0}%", Math.Round(CurrentHoney / WarehouseType.Capacity * 100)); }
        }
    }
}