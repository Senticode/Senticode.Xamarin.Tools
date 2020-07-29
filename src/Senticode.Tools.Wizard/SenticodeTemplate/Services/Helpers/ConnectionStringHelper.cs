namespace SenticodeTemplate.Services.Helpers
{
    internal static class ConnectionStringHelper
    {
        private static string GetDbFileName(ProjectSettings settings, string scope) =>
            $"Default.{settings.SavedProjectName}.{scope}.db";

        public static string GetXamarinDbFileName(ProjectSettings settings) =>
            GetDbFileName(settings, AppConstants.Mobile);

        public static string GetWebDbFileName(ProjectSettings settings) =>
            GetDbFileName(settings, AppConstants.Web);

        public static string GetSqLiteConnectionString(string dbFileName) => $"Data Source={dbFileName}";
        public static string GetPostgreConnectionString(string dbFileName) => $"Database={dbFileName}";
        public static string GetMsSqlConnectionString(string dbFileName) => $"database={dbFileName}";
        public static string GetMySqlConnectionString(string dbFileName) => $"database={dbFileName}";
    }
}