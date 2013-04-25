using Beepas.Web.Classes.IoC;

namespace Beepas.Web
{
    /// <summary>
    /// Конфигурация IoC контейнера
    /// </summary>
    public static class IoCConfig
    {
        /// <summary>
        /// Выполняет конфигурацию
        /// </summary>
        public static void Init()
        {
             Locator.Init(new WebLayer());
        }
    }
}