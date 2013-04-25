namespace Beepas.Web.Classes.Enums
{
    /// <summary>
    /// Статусы пользователей
    /// </summary>
    public enum UserStatuses: short
    {
        /// <summary>
        /// Пользователь активен
        /// </summary>
        [EnumDescription("Активен")]
        Active = 0,

        /// <summary>
        /// Пользователь заблокирован
        /// </summary>
        [EnumDescription("Заблокирован")]
        Blocked = 1
    }
}