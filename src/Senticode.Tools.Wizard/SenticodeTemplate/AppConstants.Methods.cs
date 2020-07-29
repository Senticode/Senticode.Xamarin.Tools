using System.IO;
using SenticodeTemplate.Services;

namespace SenticodeTemplate
{
    internal static partial class AppConstants
    {
        public static string GetWebApiProjectFilePath(ProjectSettings settings, string fileName) => Path.Combine(
            settings.SavedPath, Src, Web, $"{settings.SavedProjectName}.{Web}.{Api}", fileName);

        public static string GetWebDatabaseProjectFilePath(ProjectSettings settings) =>
            GetWebDatabaseProjectFilePath(settings, $"{settings.SavedProjectName}.{DataAccessWebModule}.csproj");

        public static string GetWebDatabaseProjectFilePath(ProjectSettings settings, string fileName) => Path.Combine(
            settings.SavedPath, Src, Web, Modules, $"{settings.SavedProjectName}.{DataAccessWebModule}", fileName);

        public static string GetXamarinDatabaseProjectFilePath(ProjectSettings settings) =>
            GetXamarinDatabaseProjectFilePath(settings,
                $"{settings.SavedProjectName}.{DataAccessXamarinModule}.csproj");

        public static string GetXamarinDatabaseProjectFilePath(ProjectSettings settings, string fileName) =>
            Path.Combine(settings.SavedPath, Src, Mobile, Modules,
                $"{settings.SavedProjectName}.{DataAccessXamarinModule}", fileName);

        public static string GetMobileProjectPath(ProjectSettings settings, string platform) => Path.Combine(
            settings.SavedPath, Src, Mobile, Startup, $"{settings.SavedProjectName}.{platform}");

        public static string GetMobileProjectFilePath(ProjectSettings settings, string platform, string fileName) =>
            Path.Combine(GetMobileProjectPath(settings, platform), fileName);
    }
}