using System.Linq;
using Beepas.Web.Classes.DAL;
using Beepas.Web.Classes.Entities;

namespace Beepas.Web.Classes.Managers
{
    /// <summary>
    /// Менеджер пользователей
    /// </summary>
    public class UsersManager
    {
        /// <summary>
        /// Контекст доступа к данным
        /// </summary>
        private BeepasDataContext DataContext { get; set; }

        /// <summary>
        /// Количество игроков в игре
        /// </summary>
        public int PlayersCount
        {
            get { return DataContext.Users.Count(); }
        }

        /// <summary>
        /// Стандартный конструктор с инжектированием контекст данных
        /// </summary>
        /// <param name="dataContext"></param>
        public UsersManager(BeepasDataContext dataContext)
        {
            DataContext = dataContext;
        }
        
        /// <summary>
        /// Загружает пользователя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Пользователь</returns>
        public User LoadUserById(long id)
        {
            return DataContext.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Ищет пользователя по его логину и паролю
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="passwordHash">Хеш пароля</param>
        /// <returns></returns>
        public User FindUserByLoginAndPasswordHash(string login, string passwordHash)
        {
            return DataContext.Users.FirstOrDefault(u => u.Login.ToLower() == login.ToLower() && u.PasswordHash.ToLower() == passwordHash.ToLower());
        }

        /// <summary>
        /// Сохраняет изменения в базу данных
        /// </summary>
        public void SubmitChanges()
        {
            DataContext.SubmitChanges();
        }

        /// <summary>
        /// Регистрирует пользователя в системе
        /// </summary>
        /// <param name="user"></param>
        public void RegisterUser(User user)
        {
            user.Rank = 1;
            DataContext.Users.InsertOnSubmit(user);
            SubmitChanges();
        }

        /// <summary>
        /// Возвращает пользователя по его логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public User FindUserByLogin(string login)
        {
            return DataContext.Users.FirstOrDefault(u => u.Login.ToLower() == login.ToLower());
        }
    }
}