namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// Назначение и способ получения золотых слитков
    /// </summary>
    public enum GoldBarsAppointment: short
    {
        /// <summary>
        /// Золотые слитки назначаются в банк игры
        /// </summary>
        [EnumDescription("Банк игры")]
        GameBank = 1,

        /// <summary>
        /// Золотые слитки получаются из реферального бонуса
        /// </summary>
        [EnumDescription("Реферальный бонус")]
        ReferalBonus = 2,

        /// <summary>
        /// Золотые слитки получены путем продажи меда
        /// </summary>
        [EnumDescription("Продажа меда")]
        HoneySell = 3,

        /// <summary>
        /// Золотые слитки списываются на выплаты пользователям
        /// </summary>
        [EnumDescriptionAttribute("Выплаты пользователям")]
        UserPayments = 4

    }
}