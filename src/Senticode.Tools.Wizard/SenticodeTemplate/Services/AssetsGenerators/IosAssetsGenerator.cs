using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using ProjectTemplateWizard.Abstractions.Interfaces;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.AssetsGenerators
{
    internal sealed class IosAssetsGenerator : IAssetsGenerator
    {
        private const string LaunchScreenBackgroundRegex = @"<color(.|\s)*?key=""backgroundColor""(.|\s)*?\/>";
        private static readonly string AppIconAssetFolder = Path.Combine("Assets.xcassets", "AppIcons.appiconset");
        private static readonly string LaunchScreenAssetFolder = Path.Combine("Assets.xcassets", "Splash.imageset");

        private static readonly string LaunchScreenStoryboardFile =
            Path.Combine("Resources", "LaunchScreen.storyboard");

        private static readonly IReadOnlyDictionary<string, int> AppIconAssets = new Dictionary<string, int>
        {
            {"AppStore1024.png", 1024},
            {"iPadApp76.png", 76},
            {"iPadApp152.png", 152},
            {"iPadNotification20.png", 20},
            {"iPadNotification40.png", 40},
            {"iPadProApp167.png", 167},
            {"iPadSettings29.png", 29},
            {"iPadSettings58.png", 58},
            {"iPadSpotlight40.png", 40},
            {"iPadSpotlight80.png", 80},
            {"iPhoneApp120.png", 120},
            {"iPhoneApp180.png", 180},
            {"iPhoneNotification40.png", 40},
            {"iPhoneNotification60.png", 60},
            {"iPhoneSettings87.png", 87},
            {"iPhoneSpotlight58.png", 58},
            {"iPhoneSpotlight80.png", 80},
            {"iPhoneSpotlight120.png", 120}
        };

        private static readonly IReadOnlyDictionary<string, int> LaunchScreenAssets = new Dictionary<string, int>
        {
            {"splash@3x.png", 543},
            {"splash@2x.png", 420},
            {"splash.png", 266}
        };

        public void GenerateAssets(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;
            var rootProjectPath = AppConstants.GetMobileProjectPath(settings, AppConstants.Ios);
            SetLaunchScreenBackgroundColor(data, rootProjectPath);
            GenerateAppIconAssets(data, rootProjectPath);
            GenerateLaunchScreenAssets(data, rootProjectPath);
        }

        private static void SetLaunchScreenBackgroundColor(IProjectTemplateData data, string rootProjectPath)
        {
            var color = HexRgbaConverter.ToRgbaColor<Color>(data.SplashScreenBackgroundColor);
            var launchScreenFile = Path.Combine(rootProjectPath, LaunchScreenStoryboardFile);
            var launchScreenContent = File.ReadAllText(launchScreenFile);

            launchScreenContent = Regex.Replace(launchScreenContent, LaunchScreenBackgroundRegex,
                @$"<color key=""backgroundColor"" colorSpace=""calibratedRGB"" alpha=""{color.ScA}"" red=""{color.ScR}"" green=""{color.ScG}"" blue=""{color.ScB}"" />");

            File.WriteAllText(launchScreenFile, launchScreenContent);
        }

        private static void GenerateAppIconAssets(IProjectTemplateData data, string rootPath)
        {
            var assetFolder = Path.Combine(rootPath, AppIconAssetFolder);
            var source = data.AppIconPath;
            var background = HexRgbaConverter.ToRgbaColor<System.Drawing.Color>(data.AppIconBackgroundColor);

            foreach (var pair in AppIconAssets)
            {
                var targetName = pair.Key;
                var newSize = pair.Value;
                var targetPath = Path.Combine(assetFolder, targetName);
                PngImageHelper.ResizeWithFillAndSave(source, targetPath, newSize, background);
            }
        }

        private static void GenerateLaunchScreenAssets(IProjectTemplateData data, string rootPath)
        {
            var assetFolder = Path.Combine(rootPath, LaunchScreenAssetFolder);
            var source = data.SplashScreenImagePath;

            foreach (var pair in LaunchScreenAssets)
            {
                var targetName = pair.Key;
                var newSize = pair.Value;
                var targetPath = Path.Combine(assetFolder, targetName);
                PngImageHelper.ResizeAndSave(source, targetPath, newSize);
            }
        }

        #region singleton

        private IosAssetsGenerator()
        {
        }

        public static IosAssetsGenerator Instance { get; } = new IosAssetsGenerator();

        #endregion
    }
}