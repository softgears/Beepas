using System.Linq;
using Beepas.Web.Classes.DAL;
using Beepas.Web.Classes.Entities;

namespace Beepas.Web.Classes.Managers
{
    /// <summary>
    /// Менеджер апгрейдов
    /// </summary>
    public class UpgradesManager
    {
        /// <summary>
        /// Контекст доступа к данным
        /// </summary>
        public BeepasDataContext DataContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public UpgradesManager(BeepasDataContext dataContext)
        {
            DataContext = dataContext;
        }

        /// <summary>
        /// Возвращает следующий апгрейд для улья или возвращает null, если уровень апгерйда максимален
        /// </summary>
        /// <returns></returns>
        public HiveType GetNextHiveUpgrade(Hive hive)
        {
            if (hive.HiveType.Level == 12)
            {
                return null;
            }
            return DataContext.HiveTypes.FirstOrDefault(ht => ht.Level == hive.HiveType.Level + 1);
        }

        /// <summary>
        /// Возвращает следующий апгрейд для склада или возвращает null, если уровень апгерйда максимален
        /// </summary>
        /// <returns></returns>
        public WarehouseType GetNextWarehouseUpgrade(Warehouse warehouse)
        {
            if (warehouse.WarehouseType.Level == 12)
            {
                return null;
            }
            return DataContext.WarehouseTypes.FirstOrDefault(ht => ht.Level == warehouse.WarehouseType.Level + 1);
        }

        /// <summary>
        /// Возвращает следующий апгрейд для склада или возвращает null, если уровень апгерйда максимален
        /// </summary>
        /// <returns></returns>
        public TransportType GetNextTransportUpgrade(Transport transport)
        {
            if (transport.TransportType.Level == 12)
            {
                return null;
            }
            return DataContext.TransportTypes.FirstOrDefault(ht => ht.Level == transport.TransportType.Level + 1);
        }

        /// <summary>
        /// Возвращает тип автоматического сборщика, который необходим для указанного улья
        /// </summary>
        /// <param name="hive">Улей</param>
        /// <returns></returns>
        public AutoCollectorType GetAutoCollectorForHive(Hive hive)
        {
            return DataContext.AutoCollectorTypes.FirstOrDefault(ht => ht.Level == hive.HiveType.Level);
        }
    }
}