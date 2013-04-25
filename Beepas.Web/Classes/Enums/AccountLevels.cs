namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// Уровня аккаунта
    /// </summary>
    public enum AccountLevels: short
    {
        /// <summary>
        /// Обычный пользователь
        /// </summary>
        [EnumDescription("Обычный")]
        Common = 0,

        /// <summary>
        /// ВИП пользователь
        /// </summary>
        [EnumDescription("VIP")]
        VIP = 1
    }
}