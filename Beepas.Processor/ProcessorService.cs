using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Beepas.Web.Classes.Entities;

namespace Beepas.Processor
{
    /// <summary>
    /// Класс, выполняющий непосредственную обработку действий
    /// </summary>
    public partial class ProcessorService : ServiceBase
    {
        /// <summary>
        /// Таймер, который срабатывает каждые 5 минут и обрабатывает накопление меда в ульях
        /// </summary>
        public System.Threading.Timer Honey5MinutesTimer { get; private set; }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public ProcessorService()
        {
            InitializeComponent();

            // Инициаилизруем таймер
            Honey5MinutesTimer = new Timer((state) => ProcessHoneyTick(),this,(long) 20000,60*1000*5);
        }

        /// <summary>
        /// Обрабатывает накопившийся мед
        /// </summary>
        private void ProcessHoneyTick()
        {
            // Контекст по работе с данными
            var dataContext = new Beepas.Web.Classes.DAL.BeepasDataContext();

            // Таблица хранящая соотношение, сколько опыта получил тот или иной пользователь
            var usersExp = new Dictionary<long,decimal>();

            // Изолируем список ульев
            var hives = dataContext.Hives.ToList();
            foreach (var hive in hives)
            {
                // Делаем прирост меда
                var newAmount = hive.CurrentHoney + hive.HiveType.ProductionRate;
                if (newAmount > hive.HiveType.Capacity)
                {
                    newAmount = hive.HiveType.Capacity;
                }

                // Реализуем увеличение опыта только если есть свободное место в улье
                if (!hive.IsFull)
                {
                    if (!usersExp.ContainsKey(hive.UserId))
                    {
                        usersExp[hive.UserId] = 0;
                    }
                    usersExp[hive.User.Id] += hive.HiveType.ProductionRate;
                }

                hive.CurrentHoney = newAmount;

                // Используем автосборщик если он есть
                if (hive.HasAutoCollector)
                {
                    // Проверяем нужно ли отключить автоматический сборщик
                    if (hive.AutoCollectors.Expiration.HasValue && hive.AutoCollectors.Expiration.Value <= DateTime.Now)
                    {
                        // Отключаем
                        hive.HasAutoCollector = false;
                        continue; // Уходим
                    }

                    if (hive.IsFull)
                    {
                        hive.CollectHoney();
                    }
                }
            }

            // Добавляем опыт всем пользователям, которые его получили
            foreach (var uData in usersExp)
            {
                // Увеличиваем  собранный мед
                var user = dataContext.Users.First(u => u.Id == uData.Key);
                user.TotalHoney += uData.Value;

                // Проверяем, нужно ли нам перевести пользователя в следующий ранк
                if (user.Rank < 12)
                {
                    var nextRank = dataContext.Ranks.First(r => r.Level == user.UserRank.Level + 1);
                    if (user.TotalHoney >= nextRank.RequiredHoney)
                    {
                        user.UserRank = nextRank;
                    }
                }
            }

            // Сохраняем изменения
            dataContext.SubmitChanges(ConflictMode.ContinueOnConflict); // Перетираем конфликтные изменения
        }

        /// <summary>
        /// Запуск сервиса
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {

        }

        /// <summary>
        /// Сервис остановлен
        /// </summary>
        protected override void OnStop()
        {
        }
    }
}
