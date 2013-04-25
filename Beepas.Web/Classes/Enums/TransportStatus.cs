namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// Статусы транспортных средств
    /// </summary>
    public enum TransportStatus: short
    {
        /// <summary>
        /// Транспорт свободен и ожидает
        /// </summary>
        [EnumDescription("Ожидание")]
        Waiting = 1,

        /// <summary>
        /// ТИет загрузка меда в транспорт
        /// </summary>
        [EnumDescription("Загрузка")]
        Loading = 2,

        /// <summary>
        /// Транспорт едет на биржу
        /// </summary>
        [EnumDescription("На пути к бирже")]
        OnTheWay = 3,

        /// <summary>
        /// Транспорт находится на бирже
        /// </summary>
        [EnumDescription("На бирже")]
        OnStocks = 4,

        /// <summary>
        /// Транспорт едет обратно
        /// </summary>
        [EnumDescription("На пути домой")]
        OnTheWayBack = 5
    }
}