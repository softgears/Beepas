namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// Направление платежей
    /// </summary>
    public enum PaymentDirection: short
    {
        /// <summary>
        /// Начисление
        /// </summary>
        [EnumDescription("Начисление")]
        Income = 1,

        /// <summary>
        /// Списание
        /// </summary>
        [EnumDescription("Списание")]
        Outcome = 2
    }
}