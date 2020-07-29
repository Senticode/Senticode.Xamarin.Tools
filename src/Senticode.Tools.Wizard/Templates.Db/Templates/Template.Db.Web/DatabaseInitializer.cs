using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Senticode.Database.Tools.Interfaces;

namespace Template.Db.Web
{
    internal class DatabaseInitializer
    {
        public void Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                var database = serviceProvider.GetService<IConnectionManager>().GetDbStrongContext().Database;
                database.EnsureCreated();
                //TODO enable migrations if required and remove EnsureCreated call
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        #region singleton

        private DatabaseInitializer()
        {
        }

        public static DatabaseInitializer Instance { get; } = new DatabaseInitializer();

        #endregion
    }
}