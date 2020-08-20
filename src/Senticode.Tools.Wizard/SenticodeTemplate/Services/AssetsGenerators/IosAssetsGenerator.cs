using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using SenticodeTemplate.Constants;
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

        private static readonly IReadOnlyList<AssetInfo> AppIconAssets = new List<AssetInfo>
        {
            new AssetInfo("AppStore1024.png", 1024),
            new AssetInfo("iPadApp76.png", 76),
            new AssetInfo("iPadApp152.png", 152),
            new AssetInfo("iPadNotification20.png", 20),
            new AssetInfo("iPadNotification40.png", 40),
            new AssetInfo("iPadProApp167.png", 167),
            new AssetInfo("iPadSettings29.png", 29),
            new AssetInfo("iPadSettings58.png", 58),
            new AssetInfo("iPadSpotlight40.png", 40),
            new AssetInfo("iPadSpotlight80.png", 80),
            new AssetInfo("iPhoneApp120.png", 120),
            new AssetInfo("iPhoneApp180.png", 180),
            new AssetInfo("iPhoneNotification40.png", 40),
            new AssetInfo("iPhoneNotification60.png", 60),
            new AssetInfo("iPhoneSettings87.png", 87),
            new AssetInfo("iPhoneSpotlight58.png", 58),
            new AssetInfo("iPhoneSpotlight80.png", 80),
            new AssetInfo("iPhoneSpotlight120.png", 120)
        };

        private static readonly IReadOnlyList<AssetInfo> LaunchScreenAssets = new List<AssetInfo>
        {
            new AssetInfo("splash@3x.png", 543),
            new AssetInfo("splash@2x.png", 420),
            new AssetInfo("splash.png", 266)
        };

        private static readonly IReadOnlyList<AssetInfo> ArtworkAssets = new List<AssetInfo>
        {
            new AssetInfo("iTunesArtwork", 512),
            new AssetInfo("iTunesArtwork@2x", 1024)
        };

        public void GenerateAssets(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;
            var rootProjectPath = AppConstants.GetMobileProjectPath(settings, AppConstants.Ios);
            var iconBackground = HexRgbaConverter.ToRgbaColor<System.Drawing.Color>(data.AppIconBackgroundColor);
            var splashScreenBackground = HexRgbaConverter.ToRgbaColor<Color>(data.SplashScreenBackgroundColor);
            SetLaunchScreenBackgroundColor(splashScreenBackground, rootProjectPath);
            GenerateAppIconAssets(data.AppIconPath, rootProjectPath, iconBackground);
            GenerateITunesArtwork(data.AppIconPath, rootProjectPath, iconBackground);
            GenerateLaunchScreenAssets(data.SplashScreenImagePath, rootProjectPath);
        }

        private static void SetLaunchScreenBackgroundColor(Color background, string rootProjectPath)
        {
            var launchScreenFile = Path.Combine(rootProjectPath, LaunchScreenStoryboardFile);
            var launchScreenContent = File.ReadAllText(launchScreenFile);

            launchScreenContent = Regex.Replace(launchScreenContent, LaunchScreenBackgroundRegex,
                @$"<color key=""backgroundColor"" colorSpace=""calibratedRGB"" alpha=""{background.ScA}"" red=""{background.ScR}"" green=""{background.ScG}"" blue=""{background.ScB}"" />");

            File.WriteAllText(launchScreenFile, launchScreenContent);
        }

        private static void GenerateAppIconAssets(string assetSourcePath, string rootPath,
            System.Drawing.Color background)
        {
            var assetFolder = Path.Combine(rootPath, AppIconAssetFolder);

            foreach (var asset in AppIconAssets)
            {
                var targetPath = Path.Combine(assetFolder, asset.Name);
                PngImageHelper.ResizeWithFillAndSave(assetSourcePath, targetPath, asset.Size, background);
            }
        }

        private static void GenerateLaunchScreenAssets(string assetSourcePath, string rootPath)
        {
            var assetFolder = Path.Combine(rootPath, LaunchScreenAssetFolder);

            foreach (var asset in LaunchScreenAssets)
            {
                var targetPath = Path.Combine(assetFolder, asset.Name);
                PngImageHelper.ResizeAndSave(assetSourcePath, targetPath, asset.Size);
            }
        }

        private static void GenerateITunesArtwork(string assetSourcePath, string rootPath,
            System.Drawing.Color background)
        {
            foreach (var asset in ArtworkAssets)
            {
                var targetPath = Path.Combine(rootPath, asset.Name);
                PngImageHelper.ResizeWithFillAndSave(assetSourcePath, targetPath, asset.Size, background);
            }
        }

        private sealed class AssetInfo
        {
            public AssetInfo(string name, int size)
            {
                Name = name;
                Size = size;
            }

            public string Name { get; }
            public int Size { get; }
        }

        #region singleton

        private IosAssetsGenerator()
        {
        }

        public static IosAssetsGenerator Instance { get; } = new IosAssetsGenerator();

        #endregion
    }
}