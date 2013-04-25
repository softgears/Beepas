﻿using System.Web.Mvc;
using Beepas.Web.Classes.Entities;
using Beepas.Web.Controllers;

namespace Beepas.Web.Classes.Extension
{
    /// <summary>
    /// Статический класс с расширениями вью
    /// </summary>
    public static class ViewContextExtensions
    {
        /// <summary>
        /// Текущий аутентифицированный пользователь
        /// </summary>
        /// <param name="viewContext">Контекст вью</param>
        /// <returns>Объект пользователя</returns>
        public static User CurrentUser(this ViewContext viewContext)
        {
            var baseController = viewContext.Controller as BaseController;
            if (baseController != null)
            {
                return baseController.CurrentUser;
            }
            return null;
        }

        /// <summary>
        /// Проверяет аутентифицирован ли текущий пользователь
        /// </summary>
        /// <param name="viewContext">контекст вью</param>
        /// <returns>true если да, иначе false</returns>
        public static bool IsAuthentificated(this ViewContext viewContext)
        {
            var baseController = (BaseController)viewContext.Controller as BaseController;
            if (baseController != null)
            {
                return baseController.CurrentUser != null;
            }
            return false;
        }
    }
}