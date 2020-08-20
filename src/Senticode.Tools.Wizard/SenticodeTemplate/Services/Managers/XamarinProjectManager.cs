using System;
using System.IO;
using System.Linq;
using ProjectTemplateWizard.Abstractions.Interfaces;
using SenticodeTemplate.Constants;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.AssetsGenerators;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.Managers
{
    internal class XamarinProjectManager : IProjectManager
    {
        public void Compose()
        {
            var settings = ProjectSettings.Instance;
            var data = settings.ProjectTemplateData;
            XamarinProjectHelper.AddXamarinCoreProject(settings);

            if (data.SelectedPlatforms.Contains(PlatformType.Android))
            {
                XamarinProjectHelper.AddAndroidProject(settings);
            }

            if (data.SelectedPlatforms.Contains(PlatformType.Ios))
            {
                XamarinProjectHelper.AddIosProject(settings);
            }

            if (data.SelectedPlatforms.Contains(PlatformType.Uwp))
            {
                XamarinProjectHelper.AddUwpProject(settings);
            }

            if (data.SelectedPlatforms.Contains(PlatformType.Wpf))
            {
                XamarinProjectHelper.AddWpfProject(settings);
            }

            if (data.SelectedPlatforms.Contains(PlatformType.GtkSharp))
            {
                XamarinProjectHelper.AddGtkProject(settings);
            }

            if (data.IsModularDesign)
            {
                XamarinProjectHelper.AddModuleAggregator(settings);
                XamarinProjectHelper.AddDatabaseModule(settings);
                AddCustomModules();

                if (data.IsWebBackendIncluded)
                {
                    XamarinProjectHelper.AddWebClientModule(settings);
                }
            }

            XamarinProjectHelper.AddReferenceToEntitiesProject(settings);

            if (data.IsLicensesInfoPageIncluded && data.ProjectTemplateType == ProjectTemplateType.MasterDetail)
            {
                XamarinProjectHelper.AddLicensesInfoPage(settings);
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
                    XamarinProjectHelper.AddModule(settings, module.Name);
                }
            }
        }

        private static class XamarinProjectHelper
        {
            internal static void AddDatabaseModule(ProjectSettings settings)
            {
                string token;
                var data = settings.ProjectTemplateData;

                if (data.XamarinDatabaseInfrastructureType == XamarinDatabaseInfrastructureType.None)
                {
                    return;
                }

                AddModule(settings, AppConstants.DataAccessXamarinModule, AppConstants.XamarinDbTemplateName);
                var dbFileName = ConnectionStringHelper.GetXamarinDbFileName(settings);
                var projectFile = AppConstants.GetXamarinDatabaseProjectFilePath(settings);

                var dbContextFile =
                    AppConstants.GetXamarinDatabaseProjectFilePath(settings, AppConstants.DbContextFile);

                switch (data.XamarinDatabaseInfrastructureType)
                {
                    case XamarinDatabaseInfrastructureType.SqLite:
                        token = ReplacementTokens.SqLite;
                        break;

                    default: throw new NotSupportedException();
                }

                FileHelper.UncommentCs(dbContextFile, token);
                FileHelper.UncommentXml(projectFile, token);
                FileHelper.ReplaceString(dbContextFile, ReplacementTokens.ConnectionString, dbFileName);
            }

            internal static void AddXamarinCoreProject(ProjectSettings settings)
            {
                var data = settings.ProjectTemplateData;
                string templateName = null;

                switch (data.ProjectTemplateType)
                {
                    case ProjectTemplateType.Blank:
                        templateName = AppConstants.BlankTemplateName;
                        break;

                    case ProjectTemplateType.MasterDetail:
                        templateName = AppConstants.XamarinCoreTemplateName;
                        break;

                    case ProjectTemplateType.Shell:
                        templateName = AppConstants.ShellTemplateName;
                        break;
                }

                AddMobileProject(settings, AppConstants.Core, settings.SavedProjectName,
                    templateName);
            }

            internal static void AddModule(ProjectSettings settings, string moduleName,
                string template = AppConstants.MobileModuleTemplateName)
            {
                AddMobileProject(settings, AppConstants.Modules, $"{settings.SavedProjectName}.{moduleName}", template);

                var classname = ProjectHelper.RenameModuleInitializer(settings.SavedPath, settings.SavedProjectName,
                    AppConstants.Mobile, moduleName);

                var moduleAggregatorProjectName =
                    $"{settings.SavedProjectName}.{AppConstants.Mobile}.{AppConstants.ModuleAggregator}";

                // Add modules to module aggregator.
                var maPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile, AppConstants.Core,
                    moduleAggregatorProjectName, $"{AppConstants.ModuleAggregator}.cs");

                FileHelper.InsertStringAfter(maPath, "if (!IsRegistered)", 1,
                    $"\t\t\t\t{classname}.Instance.Initialize(container);\n");

                FileHelper.InsertString(maPath, "using Unity;", $"using {settings.SavedProjectName}.{moduleName};\n");

                var moduleAggregatorProject = ProjectHelper.GetProjectByName(settings.Solution,
                    moduleAggregatorProjectName, AppConstants.Core, AppConstants.Mobile);

                var moduleProject = ProjectHelper.GetProjectByName(settings.Solution,
                    $"{settings.SavedProjectName}.{moduleName}", AppConstants.Modules, AppConstants.Mobile);

                ProjectHelper.AddProjectReference(moduleAggregatorProject, moduleProject);
            }

            internal static void AddModuleAggregator(ProjectSettings settings)
            {
                AddMobileProject(settings, AppConstants.Core,
                    $"{settings.SavedProjectName}.{AppConstants.Mobile}.{AppConstants.ModuleAggregator}",
                    AppConstants.MobileModuleAggregatorTemplateName);

                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile, AppConstants.Core,
                    settings.SavedProjectName, $"{AppConstants.AppInitializer}.cs");

                FileHelper.InsertString(path, "//1. Register modules",
                    $"\t\t\t\t{AppConstants.ModuleAggregator}.Instance.Initialize(container);\n");

                FileHelper.InsertString(path, "using Senticode.Base.Interfaces;",
                    $"using {settings.SavedProjectName}.{AppConstants.Mobile}.{AppConstants.ModuleAggregator};\n");

                path = path.Replace($"{AppConstants.AppInitializer}.cs", $"{settings.SavedProjectName}.csproj");

                FileHelper.InsertString(path, "</ItemGroup>",
                    "\n  <ItemGroup>\n    <ProjectReference Include=\"..\\"
                    + settings.SavedProjectName + $".{AppConstants.Mobile}.{AppConstants.ModuleAggregator}\\" +
                    settings.SavedProjectName +
                    $".{AppConstants.Mobile}.{AppConstants.ModuleAggregator}.csproj\" />\n  </ItemGroup>\n\n");
            }

            internal static void AddAndroidProject(ProjectSettings settings)
            {
                AddMobileProject(settings, AppConstants.Startup, $"{settings.SavedProjectName}.{AppConstants.Android}",
                    AppConstants.AndroidTemplateName);

                var path = AppConstants.GetMobileProjectFilePath(settings, AppConstants.Android, "MainActivity.cs");
                FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

                AddReferenceToCoreProject(settings, AppConstants.Android);
                AndroidAssetsGenerator.Instance.GenerateAssets(settings);
            }

            internal static void AddIosProject(ProjectSettings settings)
            {
                AddMobileProject(settings, AppConstants.Startup, $"{settings.SavedProjectName}.{AppConstants.Ios}",
                    AppConstants.IosTemplateName);

                var path = AppConstants.GetMobileProjectFilePath(settings, AppConstants.Ios, "AppDelegate.cs");
                FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

                AddReferenceToCoreProject(settings, AppConstants.Ios);
                IosAssetsGenerator.Instance.GenerateAssets(settings);
            }

            internal static void AddWpfProject(ProjectSettings settings)
            {
                AddMobileProject(settings, AppConstants.Startup, $"{settings.SavedProjectName}.{AppConstants.Wpf}",
                    AppConstants.WpfTemplateName);

                var path = AppConstants.GetMobileProjectFilePath(settings, AppConstants.Wpf, "MainWindow.xaml.cs");
                FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

                AddReferenceToCoreProject(settings, AppConstants.Wpf);
                WpfAssetsGenerator.Instance.GenerateAssets(settings);
            }

            internal static void AddGtkProject(ProjectSettings settings)
            {
                AddMobileProject(settings, AppConstants.Startup, $"{settings.SavedProjectName}.{AppConstants.Gtk}",
                    AppConstants.GtkTemplateName);

                var path = AppConstants.GetMobileProjectFilePath(settings, AppConstants.Gtk, "Program.cs");
                FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

                AddReferenceToCoreProject(settings, AppConstants.Gtk);
                GtkSharpAssetsGenerator.Instance.GenerateAssets(settings);
            }

            internal static void AddUwpProject(ProjectSettings settings)
            {
                AddMobileProject(settings, AppConstants.Startup, $"{settings.SavedProjectName}.{AppConstants.Uwp}",
                    AppConstants.UwpTemplateName);

                var path = AppConstants.GetMobileProjectFilePath(settings, AppConstants.Uwp, "MainPage.xaml.cs");
                FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);

                AddReferenceToCoreProject(settings, AppConstants.Uwp);
                UwpAssetsGenerator.Instance.GenerateAssets(settings);
            }

            private static void AddMobileProject(ProjectSettings settings, string solutionFolderName,
                string projectName, string templateName)
            {
                var templatePath = settings.Solution.GetProjectTemplate($"{templateName}.zip", "CSharp");
                var mobileFolder = ProjectHelper.AddSolutionFolder(settings.Solution, AppConstants.Mobile);
                var solutionFolder = ProjectHelper.AddSolutionFolder(mobileFolder, solutionFolderName);

                var projectPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile,
                    solutionFolderName, projectName);

                ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
            }

            private static void AddReferenceToCoreProject(ProjectSettings settings, string projectType)
            {
                var xamCoreProject = ProjectHelper.GetProjectByName(settings.Solution, settings.SavedProjectName,
                    AppConstants.Core, AppConstants.Mobile);

                var mobileProject = ProjectHelper.GetProjectByName(settings.Solution,
                    $"{settings.SavedProjectName}.{projectType}",
                    AppConstants.Startup, AppConstants.Mobile);

                ProjectHelper.AddProjectReference(mobileProject, xamCoreProject);
            }

            public static void AddWebClientModule(ProjectSettings settings)
            {
                var interfacesProjectName =
                    $"{settings.SavedProjectName}.{AppConstants.Mobile}.{AppConstants.Interfaces}";

                AddMobileProject(settings, AppConstants.Common, interfacesProjectName,
                    AppConstants.MobileInterfacesTemplateName);

                var interfacesProject = ProjectHelper.GetProjectByName(settings.Solution, interfacesProjectName,
                    AppConstants.Common, AppConstants.Mobile);

                var entitiesProjectName = $"{settings.SavedProjectName}.{AppConstants.Common}.{AppConstants.Entities}";

                var entitiesProject =
                    ProjectHelper.GetProjectByName(settings.Solution, entitiesProjectName, AppConstants.Common);

                var clientProjectName = $"{settings.SavedProjectName}.{AppConstants.WebClientModule}";

                AddModule(settings, AppConstants.WebClientModule, AppConstants.WebClientTemplateName);

                var clientProject = ProjectHelper.GetProjectByName(settings.Solution, clientProjectName,
                    AppConstants.Modules, AppConstants.Mobile);

                var infrastructureProjectName =
                    $"{settings.SavedProjectName}.{AppConstants.Common}.{AppConstants.Web}.{AppConstants.Infrastructure}";

                var infrastructureProject =
                    ProjectHelper.GetProjectByName(settings.Solution, infrastructureProjectName, AppConstants.Common);

                ProjectHelper.AddProjectReference(clientProject, interfacesProject);
                ProjectHelper.AddProjectReference(clientProject, infrastructureProject);
                ProjectHelper.AddProjectReference(interfacesProject, entitiesProject);

                EditWebClientNamespaces(settings);
                EditCoreProjectFiles(settings);
            }

            private static void EditCoreProjectFiles(ProjectSettings settings)
            {
                //AppSettings.cs
                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile,
                    AppConstants.Core, settings.SavedProjectName, "AppSettings.cs");

                FileHelper.InsertString(path, "using System;",
                    $"using {settings.SavedProjectName}.{AppConstants.Mobile}.{AppConstants.Interfaces}.Services.Web;\n");

                FileHelper.ReplaceString(path, "public class AppSettings : AppSettingsBase",
                    "public class AppSettings : AppSettingsBase, IWebClientSettings");

                FileHelper.UncommentCs(path, ReplacementTokens.WebClientRegistration);

                //AppLifeTimeManager.cs
                path = path.Replace("AppSettings.cs", "AppLifeTimeManager.cs");

                FileHelper.InsertString(path, "using Unity;",
                    $"using {settings.SavedProjectName}.{AppConstants.Mobile}.{AppConstants.Interfaces}.Services.Web;\n");

                FileHelper.ReplaceString(path, "private void InitializeModelController()",
                    AppConstants.InitializeModelControllerMethod);

                FileHelper.InsertStringAfter(path, AppConstants.InitializeModelControllerMethod, 1,
                    AppConstants.InitializeModelController);
            }

            private static void EditWebClientNamespaces(ProjectSettings settings)
            {
                var files = new[]
                {
                    "WebClientModuleInitializer.cs", Path.Combine("Services", "WeatherWebService.cs"),
                    Path.Combine("Services", "Internal", "WebClientFactory.cs")
                };

                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile,
                    AppConstants.Modules, $"{settings.SavedProjectName}.{AppConstants.WebClientModule}");

                foreach (var file in files)
                {
                    FileHelper.ReplaceString(Path.Combine(path, file), ReplacementTokens.TemplateNamespace,
                        settings.SavedProjectName);
                }

                path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile,
                    AppConstants.Common, $"{settings.SavedProjectName}.{AppConstants.Mobile}.{AppConstants.Interfaces}",
                    "Services", "Web", "IWeatherWebService.cs");

                FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);
            }

            public static void AddReferenceToEntitiesProject(ProjectSettings settings)
            {
                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile, AppConstants.Core,
                    settings.SavedProjectName, $"{settings.SavedProjectName}.csproj");

                var entitiesProjectName = $"{settings.SavedProjectName}.{AppConstants.Common}.{AppConstants.Entities}";

                FileHelper.InsertString(path, "</ItemGroup>",
                    "\n <ItemGroup>\n <ProjectReference Include =\"..\\..\\..\\Common\\" + entitiesProjectName + "\\"
                    + entitiesProjectName + ".csproj\" />\n  </ItemGroup>\n\n");
            }

            public static void AddLicensesInfoPage(ProjectSettings settings)
            {
                var baseDirectory = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile,
                    AppConstants.Core, $"{settings.SavedProjectName}");

                var viewsDirectory = Path.Combine(baseDirectory, "Views", "Menu");
                var licensesViewName = "LicensesInfoMenu";
                var viewCsPathPath = Path.Combine(viewsDirectory, $"{licensesViewName}.xaml.cs");
                var stream = File.Create(viewCsPathPath);
                stream.Close();
                FileHelper.ReplaceText($"SenticodeTemplate.TemplateFiles.{licensesViewName}cs.template",
                    viewCsPathPath);
                FileHelper.ReplaceString(viewCsPathPath, ReplacementTokens.ProjectName, settings.SavedProjectName);

                var viewXamlPath = Path.Combine(viewsDirectory, $"{licensesViewName}.xaml");
                stream = File.Create(viewXamlPath);
                stream.Close();
                FileHelper.ReplaceText($"SenticodeTemplate.TemplateFiles.{licensesViewName}.template", viewXamlPath);
                FileHelper.ReplaceString(viewXamlPath, ReplacementTokens.ProjectName, settings.SavedProjectName);

                var vmsDirectory = Path.Combine(baseDirectory, "ViewModels", "Menu");
                var licensesVmName = "LicensesMenuViewModel";
                var vmPath = Path.Combine(vmsDirectory, $"{licensesVmName}.cs");
                stream = File.Create(vmPath);
                stream.Close();
                FileHelper.ReplaceText($"SenticodeTemplate.TemplateFiles.{licensesVmName}.template", vmPath);
                FileHelper.ReplaceString(vmPath, ReplacementTokens.ProjectName, settings.SavedProjectName);

                var resourcesDirectory = Path.Combine(baseDirectory, "Resources");
                var resourcesName = "Licenses.xml";
                var resourcesPath = Path.Combine(resourcesDirectory, resourcesName);
                stream = File.Create(resourcesPath);
                stream.Close();
                FileHelper.ReplaceText($"SenticodeTemplate.TemplateFiles.{resourcesName}", resourcesPath);
                FileHelper.InsertString(Path.Combine(baseDirectory, $"{settings.SavedProjectName}.csproj"),
                    "<EmbeddedResource Include=\"Configuration\\app.config\" />",
                    $"<EmbeddedResource Include=\"Resources\\{resourcesName}\" />");

                AddCodeForLicensesInApp(settings);
            }

            private static void AddCodeForLicensesInApp(ProjectSettings settings)
            {
                var baseDirectory = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile,
                    AppConstants.Core, $"{settings.SavedProjectName}");

                var navigateCommandPath = Path.Combine(baseDirectory, "Commands", "Navigation",
                    "NavigateToMenuCommand.cs");
                FileHelper.InsertStringAfter(navigateCommandPath,
                    "_menuViews = new Dictionary<MenuKind, Func<ViewViewModelPair>>", 1,
                    $"{AppConstants.NavigateToLicenses}\n");

                var aboutMenuPath = Path.Combine(baseDirectory, "Views", "Menu", "AboutMenu.xaml");
                FileHelper.InsertString(aboutMenuPath, "<!--Main region-->", $"\t\t\t{AppConstants.LicensesMenuItem}");

                var aboutVmPath = Path.Combine(baseDirectory, "ViewModels", "Menu", "AboutMenuViewModel.cs");
                FileHelper.InsertString(aboutVmPath, "container.RegisterInstance(this);", AppConstants.LicenseInfo);
                FileHelper.InsertStringAfter(aboutVmPath, "Title = ResourceKeys.About;", 2,
                    "\t\tpublic ActionViewModel LicenseInfo { get; }\n");

                var appInitializerPath = Path.Combine(baseDirectory, "AppInitializer.cs");
                FileHelper.InsertString(appInitializerPath, "//3. ViewModels",
                    "\t\t\t\t\t.RegisterType<LicensesMenuViewModel>()\n");
                FileHelper.InsertString(appInitializerPath, "//4. Views",
                    "\t\t\t\t\t.RegisterType<LicensesInfoMenu>()\n");

                AddAdditionalLicenses(settings);
            }

            private static void AddAdditionalLicenses(ProjectSettings settings)
            {
                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Mobile, AppConstants.Core,
                    settings.SavedProjectName, "Resources", "Licenses.xml");
                var data = settings.ProjectTemplateData;

                if (data.IsWebBackendIncluded)
                {
                    FileHelper.InsertString(path, "</LicenseInfoViewModel>", AppConstants.WebLicense);
                }

                if (data.XamarinDatabaseInfrastructureType != XamarinDatabaseInfrastructureType.None)
                {
                    FileHelper.InsertString(path, "</LicenseInfoViewModel>", AppConstants.DatabaseLicenses);
                }
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