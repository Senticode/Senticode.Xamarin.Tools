using System;
using System.Collections.Generic;
using System.IO;
using ProjectTemplateWizard.Abstractions.Interfaces;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.AssetsGenerators;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.Managers
{
    internal class XamarinProjectManager : IProjectManager
    {
        private readonly Dictionary<PlatformType, Action<ProjectSettings>> _startupUpCreationActions =
            new Dictionary<PlatformType, Action<ProjectSettings>>
            {
                {PlatformType.Android, AddAndroidProject},
                {PlatformType.Ios, AddIosProject},
                {PlatformType.Uwp, AddUwpProject},
                {PlatformType.Wpf, AddWpfProject},
                {PlatformType.GtkSharp, AddGtkProject}
            };

        public void Compose()
        {
            var settings = ProjectSettings.Instance;
            var data = settings.ProjectTemplateData;
            AddXamarinCoreProject(settings);
            AddReferenceToEntitiesProject(settings);
            AddStartupProjects(settings);
            AddLicensesInfoPage(settings);

            if (data.IsModularDesign)
            {
                AddModuleAggregator(settings);
                AddDatabaseModule(settings);
                AddCustomModules();
                AddWebClientModule(settings);
            }
        }

        private void AddStartupProjects(ProjectSettings settings)
        {
            foreach (var platform in settings.ProjectTemplateData.SelectedPlatforms)
            {
                _startupUpCreationActions[platform](settings);
            }
        }

        private static void AddCustomModules()
        {
            var settings = ProjectSettings.Instance;
            var data = settings.ProjectTemplateData;

            if (data.CustomModules.Count == 0)
            {
                return;
            }

            foreach (var module in data.CustomModules)
            {
                if (module.ModuleType == ModuleType.Xamarin)
                {
                    AddModule(settings, module.Name);
                }
            }
        }

        private static void AddDatabaseModule(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;

            if (data.XamarinDatabaseInfrastructureType == XamarinDatabaseInfrastructureType.None)
            {
                return;
            }

            AddModule(settings, StringLiterals.DataAccessXamarinModule, TemplateProjectNames.XamarinDb);

            var dbFileName = ConnectionStringHelper.GetXamarinDbFileName(settings);
            var projectFile = StringLiterals.GetXamarinDatabaseProjectFilePath(settings);

            var dbContextFile =
                StringLiterals.GetXamarinDatabaseProjectFilePath(settings, FileNames.DbContextCs);

            var token = data.XamarinDatabaseInfrastructureType switch
            {
                XamarinDatabaseInfrastructureType.SqLite => ReplacementTokens.SqLite,
                _ => throw new ArgumentOutOfRangeException()
            };

            FileHelper.UncommentCs(dbContextFile, token);
            FileHelper.UncommentXml(projectFile, token);
            FileHelper.ReplaceString(dbContextFile, ReplacementTokens.ConnectionString, dbFileName);
        }

        private static void AddXamarinCoreProject(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;

            var templateName = data.ProjectTemplateType switch
            {
                ProjectTemplateType.Blank => TemplateProjectNames.Blank,
                ProjectTemplateType.MasterDetail => TemplateProjectNames.XamarinCore,
                ProjectTemplateType.Shell => TemplateProjectNames.Shell,
                _ => throw new ArgumentOutOfRangeException()
            };

            AddMobileProject(settings, StringLiterals.Core, settings.SavedProjectName,
                templateName);
        }

        private static void AddModule(ProjectSettings settings, string moduleName,
            string template = TemplateProjectNames.MobileModule)
        {
            AddMobileProject(settings, StringLiterals.Modules, $"{settings.SavedProjectName}.{moduleName}", template);

            var moduleInitializerName = ProjectHelper.RenameModuleInitializer(settings.SavedPath,
                settings.SavedProjectName,
                StringLiterals.Mobile, moduleName);

            var moduleAggregatorProjectName =
                $"{settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.ModuleAggregator}";

            // Add modules to module aggregator.
            var moduleAggregatorPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile,
                StringLiterals.Core, moduleAggregatorProjectName,
                $"{StringLiterals.ModuleAggregator}.{FileExtensions.Cs}");

            FileHelper.InsertString(moduleAggregatorPath, StringLiterals.ModulesRegistrationComment,
                $"\t\t\t\t{moduleInitializerName}.Instance.Initialize(container);\n");

            FileHelper.InsertStringAtStart(moduleAggregatorPath, $"using {settings.SavedProjectName}.{moduleName};\n");

            var moduleAggregatorProject = ProjectHelper.GetProjectByName(settings.Solution,
                moduleAggregatorProjectName, StringLiterals.Core, StringLiterals.Mobile);

            var moduleProject = ProjectHelper.GetProjectByName(settings.Solution,
                $"{settings.SavedProjectName}.{moduleName}", StringLiterals.Modules, StringLiterals.Mobile);

            ProjectHelper.AddProjectReference(moduleAggregatorProject, moduleProject);
        }

        private static void AddModuleAggregator(ProjectSettings settings)
        {
            AddMobileProject(settings, StringLiterals.Core,
                $"{settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.ModuleAggregator}",
                TemplateProjectNames.MobileModuleAggregator);

            var appInitializerPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile,
                StringLiterals.Core,
                settings.SavedProjectName, $"{StringLiterals.AppInitializer}.{FileExtensions.Cs}");

            FileHelper.InsertString(appInitializerPath, "//1. Register modules",
                $"\t\t\t\t{StringLiterals.ModuleAggregator}.Instance.Initialize(container);\n");

            FileHelper.InsertStringAtStart(appInitializerPath,
                $"using {settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.ModuleAggregator};\n");

            var projectFilePath = appInitializerPath.Replace($"{StringLiterals.AppInitializer}.{FileExtensions.Cs}",
                $"{settings.SavedProjectName}.{FileExtensions.CsProj}");

            FileHelper.InsertString(projectFilePath, StringLiterals.ItemGroupTag,
                $"\n<ItemGroup>\n<ProjectReference Include=\"..\\{settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.ModuleAggregator}\\{settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.ModuleAggregator}.{FileExtensions.CsProj}\"/>\n</ItemGroup>\n\n");
        }

        private static void AddAndroidProject(ProjectSettings settings)
        {
            AddMobileProject(settings, StringLiterals.Startup, $"{settings.SavedProjectName}.{StringLiterals.Android}",
                TemplateProjectNames.Android);

            var path = StringLiterals.GetMobileProjectFilePath(settings, StringLiterals.Android,
                FileNames.MainActivityCs);

            FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

            AddReferenceToCoreProject(settings, StringLiterals.Android);
            AndroidAssetsGenerator.Instance.GenerateAssets(settings);
        }

        private static void AddIosProject(ProjectSettings settings)
        {
            AddMobileProject(settings, StringLiterals.Startup, $"{settings.SavedProjectName}.{StringLiterals.Ios}",
                TemplateProjectNames.Ios);

            var path = StringLiterals.GetMobileProjectFilePath(settings, StringLiterals.Ios, FileNames.AppDelegateCs);
            FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

            AddReferenceToCoreProject(settings, StringLiterals.Ios);
            IosAssetsGenerator.Instance.GenerateAssets(settings);
        }

        private static void AddWpfProject(ProjectSettings settings)
        {
            AddMobileProject(settings, StringLiterals.Startup, $"{settings.SavedProjectName}.{StringLiterals.Wpf}",
                TemplateProjectNames.Wpf);

            var path = StringLiterals.GetMobileProjectFilePath(settings, StringLiterals.Wpf,
                FileNames.MainWindowXamlCs);
            FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

            AddReferenceToCoreProject(settings, StringLiterals.Wpf);
            WpfAssetsGenerator.Instance.GenerateAssets(settings);
        }

        private static void AddGtkProject(ProjectSettings settings)
        {
            AddMobileProject(settings, StringLiterals.Startup, $"{settings.SavedProjectName}.{StringLiterals.Gtk}",
                TemplateProjectNames.GtkSharp);

            var path = StringLiterals.GetMobileProjectFilePath(settings, StringLiterals.Gtk, FileNames.ProgramCs);
            FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

            AddReferenceToCoreProject(settings, StringLiterals.Gtk);
            GtkSharpAssetsGenerator.Instance.GenerateAssets(settings);
        }

        private static void AddUwpProject(ProjectSettings settings)
        {
            AddMobileProject(settings, StringLiterals.Startup, $"{settings.SavedProjectName}.{StringLiterals.Uwp}",
                TemplateProjectNames.Uwp);

            var path = StringLiterals.GetMobileProjectFilePath(settings, StringLiterals.Uwp, FileNames.MainPageXamlCs);
            FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

            AddReferenceToCoreProject(settings, StringLiterals.Uwp);
            UwpAssetsGenerator.Instance.GenerateAssets(settings);
        }

        private static void AddMobileProject(ProjectSettings settings, string solutionFolderName,
            string projectName, string templateName)
        {
            var templatePath =
                settings.Solution.GetProjectTemplate($"{templateName}.{FileExtensions.Zip}", LanguageNames.CSharp);

            var mobileFolder = ProjectHelper.AddSolutionFolder(settings.Solution, StringLiterals.Mobile);
            var solutionFolder = ProjectHelper.AddSolutionFolder(mobileFolder, solutionFolderName);

            var projectPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile,
                solutionFolderName, projectName);

            ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
        }

        private static void AddReferenceToCoreProject(ProjectSettings settings, string projectType)
        {
            var xamCoreProject = ProjectHelper.GetProjectByName(settings.Solution, settings.SavedProjectName,
                StringLiterals.Core, StringLiterals.Mobile);

            var mobileProject = ProjectHelper.GetProjectByName(settings.Solution,
                $"{settings.SavedProjectName}.{projectType}",
                StringLiterals.Startup, StringLiterals.Mobile);

            ProjectHelper.AddProjectReference(mobileProject, xamCoreProject);
        }

        private static void AddWebClientModule(ProjectSettings settings)
        {
            if (!settings.ProjectTemplateData.IsWebBackendIncluded)
            {
                return;
            }

            var interfacesProjectName =
                $"{settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.Interfaces}";

            AddMobileProject(settings, StringLiterals.Common, interfacesProjectName,
                TemplateProjectNames.MobileInterfaces);

            var interfacesProject = ProjectHelper.GetProjectByName(settings.Solution, interfacesProjectName,
                StringLiterals.Common, StringLiterals.Mobile);

            var entitiesProjectName = $"{settings.SavedProjectName}.{StringLiterals.Common}.{StringLiterals.Entities}";

            var entitiesProject =
                ProjectHelper.GetProjectByName(settings.Solution, entitiesProjectName, StringLiterals.Common);

            var clientProjectName = $"{settings.SavedProjectName}.{StringLiterals.WebClientModule}";

            AddModule(settings, StringLiterals.WebClientModule, TemplateProjectNames.WebClient);

            var clientProject = ProjectHelper.GetProjectByName(settings.Solution, clientProjectName,
                StringLiterals.Modules, StringLiterals.Mobile);

            var infrastructureProjectName =
                $"{settings.SavedProjectName}.{StringLiterals.Common}.{StringLiterals.Web}.{StringLiterals.Infrastructure}";

            var infrastructureProject =
                ProjectHelper.GetProjectByName(settings.Solution, infrastructureProjectName, StringLiterals.Common);

            ProjectHelper.AddProjectReference(clientProject, interfacesProject);
            ProjectHelper.AddProjectReference(clientProject, infrastructureProject);
            ProjectHelper.AddProjectReference(interfacesProject, entitiesProject);

            EditWebClientNamespaces(settings);
            EditCoreProjectFiles(settings);
        }

        private static void EditCoreProjectFiles(ProjectSettings settings)
        {
            // AppSettings.cs
            var appSettingsPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile,
                StringLiterals.Core, settings.SavedProjectName, FileNames.AppSettingsCs);

            FileHelper.InsertStringAtStart(appSettingsPath,
                $"using {settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.Interfaces}.Services.Web;\n");

            FileHelper.ReplaceString(appSettingsPath, "public class AppSettings : AppSettingsBase",
                "public class AppSettings : AppSettingsBase, IWebClientSettings");

            FileHelper.UncommentCs(appSettingsPath, ReplacementTokens.WebClientRegistration);

            // AppLifeTimeManager.cs
            var appLifeTimeManagerPath =
                appSettingsPath.Replace(FileNames.AppSettingsCs, FileNames.AppLifeTimeManagerCs);

            FileHelper.InsertStringAtStart(appLifeTimeManagerPath,
                $"using {settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.Interfaces}.Services.Web;\n");

            FileHelper.ReplaceString(appLifeTimeManagerPath, "private void InitializeModelController()",
                CodeConstants.InitializeModelControllerMethod);

            FileHelper.InsertStringAfter(appLifeTimeManagerPath, CodeConstants.InitializeModelControllerMethod, 1,
                CodeConstants.InitializeModelController);
        }

        private static void EditWebClientNamespaces(ProjectSettings settings)
        {
            var files = new[]
            {
                FileNames.WebClientModuleInitializerCs,
                Path.Combine(StringLiterals.Services, FileNames.WeatherWebServiceCs),
                Path.Combine(StringLiterals.Services, StringLiterals.Internal, FileNames.WebClientFactoryCs)
            };

            var webClientModulePath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile,
                StringLiterals.Modules, $"{settings.SavedProjectName}.{StringLiterals.WebClientModule}");

            foreach (var file in files)
            {
                FileHelper.ReplaceString(Path.Combine(webClientModulePath, file), ReplacementTokens.TemplateNamespace,
                    settings.SavedProjectName);
            }

            var weatherWebServiceInterfacePath = Path.Combine(settings.SavedPath, StringLiterals.Src,
                StringLiterals.Mobile,
                StringLiterals.Common,
                $"{settings.SavedProjectName}.{StringLiterals.Mobile}.{StringLiterals.Interfaces}",
                StringLiterals.Services, StringLiterals.Web, FileNames.InterfaceWeatherWebServiceCs);

            FileHelper.ReplaceString(weatherWebServiceInterfacePath, ReplacementTokens.TemplateNamespace,
                settings.SavedProjectName);
        }

        private static void AddReferenceToEntitiesProject(ProjectSettings settings)
        {
            var path = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile, StringLiterals.Core,
                settings.SavedProjectName, $"{settings.SavedProjectName}.{FileExtensions.CsProj}");

            var entitiesProjectName = $"{settings.SavedProjectName}.{StringLiterals.Common}.{StringLiterals.Entities}";

            FileHelper.InsertString(path, StringLiterals.ItemGroupTag,
                $"\n<ItemGroup>\n<ProjectReference Include=\"..\\..\\..\\Common\\{entitiesProjectName}\\{entitiesProjectName}.{FileExtensions.CsProj}\"/>\n</ItemGroup>\n\n");
        }

        private static void AddLicensesInfoPage(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;

            if (!(data.IsLicensesInfoPageIncluded && data.ProjectTemplateType == ProjectTemplateType.MasterDetail))
            {
                return;
            }

            var baseDirectory = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile,
                StringLiterals.Core, settings.SavedProjectName);

            var viewsDirectory = Path.Combine(baseDirectory, StringLiterals.Views, StringLiterals.Menu);

            var licensesInfoMenuCsPath =
                Path.Combine(viewsDirectory, $"{StringLiterals.LicensesInfoMenu}.{FileExtensions.XamlCs}");

            using (File.Create(licensesInfoMenuCsPath))
            {
            }

            FileHelper.ReplaceText(FileNames.LicensesInfoMenuCsTemplate, licensesInfoMenuCsPath);

            FileHelper.ReplaceString(licensesInfoMenuCsPath, ReplacementTokens.ProjectName, settings.SavedProjectName);

            var licensesInfoMenuXamlPath =
                Path.Combine(viewsDirectory, $"{StringLiterals.LicensesInfoMenu}.{FileExtensions.Xaml}");

            using (File.Create(licensesInfoMenuXamlPath))
            {
            }

            FileHelper.ReplaceText(FileNames.LicensesInfoMenuXamlTemplate, licensesInfoMenuXamlPath);

            FileHelper.ReplaceString(licensesInfoMenuXamlPath, ReplacementTokens.ProjectName,
                settings.SavedProjectName);

            var viewModelPath = Path.Combine(baseDirectory, StringLiterals.ViewModels, StringLiterals.Menu,
                FileNames.LicensesMenuViewModelCs);

            using (File.Create(viewModelPath))
            {
            }

            FileHelper.ReplaceText(FileNames.LicensesMenuViewModelTemplate,
                viewModelPath);

            FileHelper.ReplaceString(viewModelPath, ReplacementTokens.ProjectName, settings.SavedProjectName);

            var resourcesPath = Path.Combine(baseDirectory, StringLiterals.Resources, FileNames.LicensesXml);

            using (File.Create(resourcesPath))
            {
            }

            FileHelper.ReplaceText(FileNames.LicensesXmlTemplate, resourcesPath);

            FileHelper.UncommentXml(Path.Combine(baseDirectory, $"{settings.SavedProjectName}.{FileExtensions.CsProj}"),
                ReplacementTokens.Licenses);

            AddCodeForLicensesInApp(settings);
        }

        private static void AddCodeForLicensesInApp(ProjectSettings settings)
        {
            var baseDirectory = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile,
                StringLiterals.Core, settings.SavedProjectName);

            var navigateCommandPath = Path.Combine(baseDirectory, StringLiterals.Commands, StringLiterals.Navigation,
                FileNames.NavigateToMenuCommandCs);

            FileHelper.InsertStringAfter(navigateCommandPath,
                "_menuViews = new Dictionary<MenuKind, Func<ViewViewModelPair>>", 1,
                CodeConstants.NavigateToLicenses);

            var aboutMenuPath = Path.Combine(baseDirectory, StringLiterals.Views, StringLiterals.Menu,
                FileNames.AboutMenuXaml);

            FileHelper.InsertString(aboutMenuPath, StringLiterals.MainRegionXamlComment,
                CodeConstants.LicensesMenuItem);

            var aboutViewModelPath = Path.Combine(baseDirectory, StringLiterals.ViewModels, StringLiterals.Menu,
                FileNames.AboutMenuViewModelCs);

            FileHelper.UncommentCs(aboutViewModelPath, ReplacementTokens.LicensesInfo);
            FileHelper.UncommentCs(aboutViewModelPath, ReplacementTokens.LicensesInfoProperty);

            FileHelper.InsertStringAtStart(aboutViewModelPath,
                $"using {settings.SavedProjectName}.Commands.Navigation;");

            FileHelper.InsertStringAtStart(aboutViewModelPath,
                $"using {settings.SavedProjectName}.ViewModels.Abstractions;");

            var appInitializerPath = Path.Combine(baseDirectory, FileNames.AppInitializerCs);

            FileHelper.InsertString(appInitializerPath, "//3. ViewModels",
                ".RegisterType<LicensesMenuViewModel>()");

            FileHelper.InsertString(appInitializerPath, "//4. Views",
                ".RegisterType<LicensesInfoMenu>()");

            AddAdditionalLicenses(settings);
        }

        private static void AddAdditionalLicenses(ProjectSettings settings)
        {
            var path = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Mobile, StringLiterals.Core,
                settings.SavedProjectName, StringLiterals.Resources, FileNames.LicensesXml);

            var data = settings.ProjectTemplateData;

            if (data.IsWebBackendIncluded)
            {
                FileHelper.InsertString(path, StringLiterals.LicenseInfoViewModelTag, CodeConstants.WebLicense);
            }

            if (data.XamarinDatabaseInfrastructureType != XamarinDatabaseInfrastructureType.None)
            {
                FileHelper.InsertString(path, StringLiterals.LicenseInfoViewModelTag, CodeConstants.DatabaseLicenses);
            }
        }

        #region singleton

        private XamarinProjectManager()
        {
        }

        public static XamarinProjectManager Instance { get; } = new XamarinProjectManager();

        #endregion
    }
}