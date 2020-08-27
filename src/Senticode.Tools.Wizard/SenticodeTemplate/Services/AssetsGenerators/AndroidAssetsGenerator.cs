using System.Collections.Generic;
using System.IO;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.AssetsGenerators
{
    internal sealed class AndroidAssetsGenerator : IAssetsGenerator
    {
        private const string AppIconBackgroundColorToken = "@color/colorIconDefaultBackground";
        private const string SplashScreenBackgroundColorToken = "@color/colorSplashScreenDefaultBackground";
        private const string AppIconForeground = "icon_foreground.png";
        private const string AppIconRoundForeground = "icon_round_foreground.png";
        private const string SplashScreenAsset = "splash.png";

        private static readonly IReadOnlyList<AssetInfo> MipmapFolders = new List<AssetInfo>
        {
            new AssetInfo(Path.Combine(StringLiterals.Resources, "mipmap-ldpi"), 0.75d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "mipmap-mdpi"), 1d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "mipmap-hdpi"), 1.5d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "mipmap-xhdpi"), 2d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "mipmap-xxhdpi"), 3d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "mipmap-xxxhdpi"), 1)
        };

        private static readonly IReadOnlyList<AssetInfo> DrawableFolders = new List<AssetInfo>
        {
            new AssetInfo(Path.Combine(StringLiterals.Resources, "drawable-ldpi"), 0.75d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "drawable-mdpi"), 1d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "drawable-hdpi"), 1.5d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "drawable-xhdpi"), 2d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "drawable-xxhdpi"), 3d / 4d),
            new AssetInfo(Path.Combine(StringLiterals.Resources, "drawable-xxxhdpi"), 1)
        };

        private readonly string _colorFile = Path.Combine(StringLiterals.Resources, "values", "colors.xml");

        public void GenerateAssets(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;
            var rootPath = StringLiterals.GetMobileProjectPath(settings, StringLiterals.Android);
            var colorFile = Path.Combine(rootPath, _colorFile);
            FileHelper.ReplaceString(colorFile, AppIconBackgroundColorToken, data.AppIconBackgroundColor);
            FileHelper.ReplaceString(colorFile, SplashScreenBackgroundColorToken, data.SplashScreenBackgroundColor);
            GenerateAssets(data.AppIconPath, rootPath, AppIconForeground, MipmapFolders);
            GenerateAssets(data.AppIconPath, rootPath, AppIconRoundForeground, MipmapFolders);
            GenerateAssets(data.SplashScreenImagePath, rootPath, SplashScreenAsset, DrawableFolders);
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