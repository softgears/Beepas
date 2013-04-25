namespace Beepas.Web.Models.Account
{
    /// <summary>
    /// Модель входа на сайт
    /// </summary>
    public class LoginModel
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
        /// Запомнить меня на сайте
        /// </summary>
        public bool RememberMe { get; set; }
    }
}