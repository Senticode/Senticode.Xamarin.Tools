using System;
using System.IO;

namespace _template.Db.Xamarin.Helpers
{
    internal static class LocalStorageHelper
    {
        public static string GetDatabasePath(string filename)
        {
#if __IOS__
            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            var path = Path.Combine(libFolder, filename);
            return path;
#else
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
#endif
        }
    }
}