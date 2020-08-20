using System.IO;
using System.Text.RegularExpressions;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.AssetsGenerators
{
    internal sealed class UwpAssetsGenerator : IAssetsGenerator
    {
        private static readonly string LeftGroup = nameof(LeftGroup);
        private static readonly string RightGroup = nameof(RightGroup);

        private static readonly string SplashScreenBackgroundRegex =
            $@"(?<{LeftGroup}><uap:SplashScreen(.|\s)*?BackgroundColor="").*(?<{RightGroup}>""(.|\s)*?>)";

        private static readonly string AppIconBackgroundRegex =
            $@"(?<{LeftGroup}><uap:VisualElements(.|\s)*?BackgroundColor="").*(?<{RightGroup}>""(.|\s)*?>)";

        public void GenerateAssets(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;
            var rootPath = AppConstants.GetMobileProjectPath(settings, AppConstants.Uwp);
            var manifestFile = Path.Combine(rootPath, "Package.appxmanifest");
            var manifestContent = File.ReadAllText(manifestFile);
            var splashColor = HexRgbaConverter.ToHexWithNoAlpha(data.SplashScreenBackgroundColor);
            var iconColor = HexRgbaConverter.ToHexWithNoAlpha(data.AppIconBackgroundColor);

            manifestContent = Regex.Replace(manifestContent, SplashScreenBackgroundRegex,
                x => $"{x.Groups[LeftGroup].Value}{splashColor}{x.Groups[RightGroup]}");

            manifestContent = Regex.Replace(manifestContent, AppIconBackgroundRegex,
                x => $"{x.Groups[LeftGroup].Value}{iconColor}{x.Groups[RightGroup]}");

            File.WriteAllText(manifestFile, manifestContent);
        }

        #region singleton

        private UwpAssetsGenerator()
        {
        }

        public static UwpAssetsGenerator Instance { get; } = new UwpAssetsGenerator();

        #endregion
    }
}