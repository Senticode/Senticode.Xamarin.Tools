﻿using System;
using System.IO;
using System.Reflection;

namespace ProjectTemplateWizard.ViewModels.Main
{
    internal partial class MainViewModel
    {
        private const int MinAppIconImageSize = 1024;
        private const int MaxAppIconImageSize = 1024;
        private const int MinSplashScreenImageSize = 1024;
        private const int MaxSplashScreenImageSize = 1024;

        private static readonly string
            DefaultAsset = Path.Combine(Assembly.GetExecutingAssembly().Location, @"..\icon.png");

        public AssetPickerViewModel IconPickerViewModel { get; private set; }
        public AssetPickerViewModel SplashScreenImagePickerViewModel { get; private set; }

        private void InitializeAssetPickers()
        {
            if (MinAppIconImageSize > MaxAppIconImageSize ||
                MinSplashScreenImageSize > MaxSplashScreenImageSize ||
                MinAppIconImageSize < 0 ||
                MinSplashScreenImageSize < 0)
            {
                throw new NotSupportedException();
            }

            IconPickerViewModel = new AssetPickerViewModel(
                "App icon", "Select app icon", MinAppIconImageSize, MaxAppIconImageSize);

            SplashScreenImagePickerViewModel = new AssetPickerViewModel(
                "Splash screen", "Select splash screen image", MinSplashScreenImageSize, MaxSplashScreenImageSize);

            IconPickerViewModel.AssetPath = DefaultAsset;
            SplashScreenImagePickerViewModel.AssetPath = DefaultAsset;
        }
    }
}