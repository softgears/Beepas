using Autofac;
using Autofac.Integration.Mvc;
using Beepas.Web.Classes.DAL;
using Beepas.Web.Classes.Managers;

namespace Beepas.Web.Classes.IoC
{
    /// <summary>
    /// Модель регистрации зависимостей
    /// </summary>
    public class WebLayer: Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be
        ///             registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BeepasDataContext>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<UsersManager>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<MoneyManager>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<UpgradesManager>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<StatsManager>().AsSelf().InstancePerHttpRequest();
            builder.RegisterControllers(this.ThisAssembly);
        }
    }
}