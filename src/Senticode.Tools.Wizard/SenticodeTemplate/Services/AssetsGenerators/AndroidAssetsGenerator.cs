using System.Collections.Generic;
using System.IO;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.AssetsGenerators
{
    internal sealed class AndroidAssetsGenerator : IAssetsGenerator
    {
        private const string ResourceFolder = "Resources";
        private const string AppIconBackgroundColorToken = "@color/colorIconDefaultBackground";
        private const string SplashScreenBackgroundColorToken = "@color/colorSplashScreenDefaultBackground";
        private const string AppIconAssetName = "icon.png";
        private const string AdaptiveIconAssetName = "icon_foreground.png";
        private const string SplashScreenAssetName = "splash.png";

        private static readonly IReadOnlyList<AssetInfo> MipmapFolders = new List<AssetInfo>
        {
            //{Path.Combine(ResourcesFolder, "drawable-ldpi"), 0.75d / 4d}, //todo VDE: deal with it
            new AssetInfo(Path.Combine(ResourceFolder, "mipmap-mdpi"), 1d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "mipmap-hdpi"), 1.5d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "mipmap-xhdpi"), 2d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "mipmap-xxhdpi"), 3d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "mipmap-xxxhdpi"), 1)
        };

        private static readonly IReadOnlyList<AssetInfo> DrawableFolders = new List<AssetInfo>
        {
            new AssetInfo(Path.Combine(ResourceFolder, "drawable-ldpi"), 0.75d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "drawable-mdpi"), 1d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "drawable-hdpi"), 1.5d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "drawable-xhdpi"), 2d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "drawable-xxhdpi"), 3d / 4d),
            new AssetInfo(Path.Combine(ResourceFolder, "drawable-xxxhdpi"), 1)
        };

        private readonly string _colorFile = Path.Combine(ResourceFolder, "values", "colors.xml");

        public void GenerateAssets(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;
            var rootPath = AppConstants.GetMobileProjectPath(settings, AppConstants.Android);
            var colorFile = Path.Combine(rootPath, _colorFile);
            FileHelper.ReplaceString(colorFile, AppIconBackgroundColorToken, data.AppIconBackgroundColor);
            FileHelper.ReplaceString(colorFile, SplashScreenBackgroundColorToken, data.SplashScreenBackgroundColor);
            GenerateAssets(data.AppIconPath, rootPath, AppIconAssetName, MipmapFolders);
            GenerateAssets(data.AppIconPath, rootPath, AdaptiveIconAssetName, MipmapFolders);
            GenerateAssets(data.SplashScreenImagePath, rootPath, SplashScreenAssetName, DrawableFolders);
        }

        private static void GenerateAssets(string sourcePath, string rootPath, string assetName,
            IEnumerable<AssetInfo> assets)
        {
            foreach (var asset in assets)
            {
                PngImageHelper.ScaleAndSave(sourcePath, Path.Combine(rootPath, asset.Path, assetName), asset.Scale);
            }
        }

        private sealed class AssetInfo
        {
            public AssetInfo(string path, double scale)
            {
                Path = path;
                Scale = scale;
            }

            public string Path { get; }
            public double Scale { get; }
        }

        #region singleton

        private AndroidAssetsGenerator()
        {
        }

        public static AndroidAssetsGenerator Instance { get; } = new AndroidAssetsGenerator();

        #endregion
    }
}