namespace Beepas.Web.Models.Account
{
    /// <summary>
    /// Модель регистрации пользователя
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        public string PasswordConfirm { get; set; }

        /// <summary>
        /// Мыло
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Идентификатор пользователя реферала
        /// </summary>
        public long? ReffererId { get; set; }
    }
}