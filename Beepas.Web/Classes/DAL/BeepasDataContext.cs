﻿namespace Beepas.Web.Classes.DAL
{
    /// <summary>
    /// Контекст доступа к данным
    /// </summary>
    public partial class BeepasDataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Data.Linq.DataContext"/> class by referencing a file source.
        /// </summary>
        /// <param name="fileOrServerOrConnection">This argument can be any one of the following:The name of a file where a SQL Server Express database resides.The name of a server where a database is present. In this case the provider uses the default database for a user.A complete connection string. LINQ to SQL just passes the string to the provider without modification.</param>
        public BeepasDataContext() : base(System.Configuration.ConfigurationManager.ConnectionStrings["BeepasConnectionString"].ConnectionString)
        {
        }
    }
}