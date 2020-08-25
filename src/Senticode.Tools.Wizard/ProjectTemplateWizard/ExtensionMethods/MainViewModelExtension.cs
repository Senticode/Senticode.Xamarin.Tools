using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectTemplateWizard.Abstractions.Interfaces;
using ProjectTemplateWizard.Models;
using ProjectTemplateWizard.ViewModels.Main;

namespace ProjectTemplateWizard.ExtensionMethods
{
    internal static class MainViewModelExtension
    {
        public static IProjectTemplateData ConvertToProjectTemplateData(this MainViewModel viewModel)
        {
            var selectedPlatforms = new List<PlatformType>();

            if (viewModel.IsAndroidPlatformSelected)
            {
                selectedPlatforms.Add(PlatformType.Android);
            }

            if (viewModel.IsIosPlatformSelected)
            {
                selectedPlatforms.Add(PlatformType.Ios);
            }

            if (viewModel.IsUwpPlatformSelected)
            {
                selectedPlatforms.Add(PlatformType.Uwp);
            }

            if (viewModel.IsWpfPlatformSelected)
            {
                selectedPlatforms.Add(PlatformType.Wpf);
            }

            if (viewModel.IsGtkSharpPlatformSelected)
            {
                selectedPlatforms.Add(PlatformType.GtkSharp);
            }

            var appIconPath = viewModel.IconPickerViewModel.AssetPath;

            if (string.IsNullOrWhiteSpace(appIconPath) || !File.Exists(appIconPath))
            {
                throw new NotSupportedException($"Asset source does not exist {nameof(appIconPath)}");
            }

            var splashScreenImagePath = viewModel.SplashScreenImagePickerViewModel.AssetPath;

            if (string.IsNullOrWhiteSpace(splashScreenImagePath) || !File.Exists(splashScreenImagePath))
            {
                throw new NotSupportedException($"Asset source does not exist {nameof(splashScreenImagePath)}");
            }

            IReadOnlyList<ModuleInfo> customModules = null;

            if (viewModel.IsModularDesign)
            {
                if (viewModel.IsWebBackendIncluded)
                {
                    customModules = viewModel.CustomModules.Select(x => x.ToModelInfo()).ToList();
                }
                else
                {
                    customModules = viewModel.CustomModules.Where(x => x.ModuleType != ModuleType.Web)
                        .Select(x => x.ToModelInfo()).ToList();
                }
            }

            var result = new ProjectTemplateData
            {
                SplashScreenImagePath = splashScreenImagePath,
                AppIconPath = appIconPath,
                SplashScreenBackgroundColor = viewModel.SplashScreenImagePickerViewModel.SelectedColor.ToString(),
                AppIconBackgroundColor = viewModel.IconPickerViewModel.SelectedColor.ToString(),
                ProjectTemplateType = viewModel.ProjectTemplateType,
                CustomModules = customModules,
                IsModularDesign = viewModel.IsModularDesign,
                SelectedPlatforms = selectedPlatforms,
                IsNUnitIncluded = viewModel.IsNUnitIncluded,
                IsWebBackendIncluded = viewModel.WebBackendModule.IsApplied,
                IsDockerComposeIncluded = viewModel.IsDockerComposeIncluded,
                IsSignalRIncluded = viewModel.SignalRModule.IsApplied,
                IsSwaggerIncluded = viewModel.IsSwaggerIncluded,
                IsIdentityServerIncluded = viewModel.IsIdentityServerIncluded,

                XamarinDatabaseInfrastructureType = viewModel.DatabaseXamarinModule.IsApplied
                    ? viewModel.XamarinDatabaseInfrastructureType
                    : XamarinDatabaseInfrastructureType.None,

                WebDatabaseInfrastructureType = viewModel.DatabaseWebModule.IsApplied
                    ? viewModel.WebDatabaseInfrastructureType
                    : WebDatabaseInfrastructureType.None,

                IsReadmeIncluded = viewModel.IsReadmeIncluded,
                IsVersioningSystemIncluded = viewModel.IsVersioningSystemIncluded,
                IsLicensesInfoPageIncluded = viewModel.IsLicensesInfoPageIncluded,
            };

            return result;
        }

        public static void UpdatePlatformSelection(this MainViewModel viewModel)
        {
            viewModel.IsAnyPlatformSelected = viewModel.IsAndroidPlatformSelected ||
                                              viewModel.IsGtkSharpPlatformSelected ||
                                              viewModel.IsIosPlatformSelected ||
                                              viewModel.IsUwpPlatformSelected ||
                                              viewModel.IsWpfPlatformSelected;
        }

        public static void UpdateWebBackendSelection(this MainViewModel viewModel)
        {
            viewModel.WebBackendModule.IsApplied = viewModel.IsWebBackendIncluded;

            if (viewModel.IsWebBackendIncluded)
            {
                return;
            }

            if (viewModel.SelectedModule?.ModuleType == ModuleType.Web)
            {
                viewModel.SelectedModule = null;
            }

            viewModel.IsDockerComposeIncluded = false;
            viewModel.IsSwaggerIncluded = false;
            viewModel.IsIdentityServerIncluded = false;
            viewModel.DatabaseWebModule.IsApplied = false;
            viewModel.SignalRModule.IsApplied = false;

            if (viewModel.IsModuleEditModeOn)
            {
                viewModel.ModuleEditingViewModel.IsXamarinModule = true;
            }
        }

        public static void UpdateArchitecturePatterSelection(this MainViewModel viewModel)
        {
            if (viewModel.IsModularDesign)
            {
                return;
            }

            viewModel.SelectedModule = null;
            viewModel.IsModuleEditModeOn = false;
        }
    }
}