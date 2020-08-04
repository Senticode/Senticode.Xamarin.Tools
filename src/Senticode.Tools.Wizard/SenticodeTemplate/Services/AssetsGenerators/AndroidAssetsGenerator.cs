using System.Collections.Generic;
using System.IO;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.AssetsGenerators
{
    internal sealed class AndroidAssetsGenerator : IAssetsGenerator
    {
        private const string ResourcesFolder = "Resources";
        private const string AppIconBackgroundColorToken = "@color/colorIconDefaultBackground";
        private const string SplashScreenBackgroundColorToken = "@color/colorSplashScreenDefaultBackground";
        private const string AppIconAssetName = "icon.png";
        private const string AdaptiveIconAssetName = "icon_foreground.png";
        private const string SplashScreenAssetName = "splash.png";

        private static readonly IReadOnlyDictionary<string, double> MipmapFolders = new Dictionary<string, double>
        {
            //{Path.Combine(ResourcesFolder, "drawable-ldpi"), 0.75d / 4d},
            {Path.Combine(ResourcesFolder, "mipmap-mdpi"), 1d / 4d},
            {Path.Combine(ResourcesFolder, "mipmap-hdpi"), 1.5d / 4d},
            {Path.Combine(ResourcesFolder, "mipmap-xhdpi"), 2d / 4d},
            {Path.Combine(ResourcesFolder, "mipmap-xxhdpi"), 3d / 4d},
            {Path.Combine(ResourcesFolder, "mipmap-xxxhdpi"), 1}
        };

        private static readonly IReadOnlyDictionary<string, double> DrawableFolders = new Dictionary<string, double>
        {
            {Path.Combine(ResourcesFolder, "drawable-ldpi"), 0.75d / 4d},
            {Path.Combine(ResourcesFolder, "drawable-mdpi"), 1d / 4d},
            {Path.Combine(ResourcesFolder, "drawable-hdpi"), 1.5d / 4d},
            {Path.Combine(ResourcesFolder, "drawable-xhdpi"), 2d / 4d},
            {Path.Combine(ResourcesFolder, "drawable-xxhdpi"), 3d / 4d},
            {Path.Combine(ResourcesFolder, "drawable-xxxhdpi"), 1}
        };

        private readonly string _colorsFile = Path.Combine(ResourcesFolder, "values", "colors.xml");

        public void GenerateAssets(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;
            var rootPath = AppConstants.GetMobileProjectPath(settings, AppConstants.Android);
            var colorsFile = Path.Combine(rootPath, _colorsFile);
            FileHelper.ReplaceString(colorsFile, AppIconBackgroundColorToken, data.AppIconBackgroundColor);
            FileHelper.ReplaceString(colorsFile, SplashScreenBackgroundColorToken, data.SplashScreenBackgroundColor);
            GenerateAssets(data.AppIconPath, rootPath, AppIconAssetName, false);
            GenerateAssets(data.AppIconPath, rootPath, AdaptiveIconAssetName, false);
            GenerateAssets(data.SplashScreenImagePath, rootPath, SplashScreenAssetName, true);
        }

        private static void GenerateAssets(string sourcePath, string rootPath, string assetName, bool isDrawableAssets)
        {
            if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
            {
                return;
            }

            var assetFolders = isDrawableAssets ? DrawableFolders : MipmapFolders;

            foreach (var pair in assetFolders)
            {
                var folder = pair.Key;
                var scaleFactor = pair.Value;
                PngImageHelper.ScaleAndSave(sourcePath, Path.Combine(rootPath, folder, assetName), scaleFactor);
            }
        }

        #region singleton

        private AndroidAssetsGenerator()
        {
        }

        public static AndroidAssetsGenerator Instance { get; } = new AndroidAssetsGenerator();

        #endregion
    }
}