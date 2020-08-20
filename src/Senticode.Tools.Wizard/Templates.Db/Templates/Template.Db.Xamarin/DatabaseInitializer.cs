using System;
using Senticode.Database.Tools.Interfaces;
using Unity;

namespace _template.Db.Xamarin
{
    internal class DatabaseInitializer
    {
        public void Initialize(IUnityContainer container)
        {
            try
            {
                var database = container.Resolve<IConnectionManager>().GetDbStrongContext().Database;
                database.EnsureCreated();
                //TODO enable migrations if required and remove EnsureCreated call
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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