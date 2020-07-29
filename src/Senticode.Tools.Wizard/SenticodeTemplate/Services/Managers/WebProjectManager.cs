using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectTemplateWizard.Abstractions.Interfaces;
using SenticodeTemplate.Interfaces;
using SenticodeTemplate.Services.Helpers;

namespace SenticodeTemplate.Services.Managers
{
    internal class WebProjectManager : IProjectManager
    {
        public void Compose()
        {
            var settings = ProjectSettings.Instance;
            var data = settings.ProjectTemplateData;

            if (!data.IsWebBackendIncluded)
            {
                return;
            }

            WebProjectHelper.AddWebProject(settings);

            if (data.IsModularDesign)
            {
                WebProjectHelper.AddModuleAggregator(settings);
                WebProjectHelper.AddDatabaseModule(settings);
                AddModules();
            }

            if (data.IsSwaggerIncluded)
            {
                WebProjectHelper.AddSwagger(settings);
            }

            if (data.IsDockerComposeIncluded)
            {
                WebProjectHelper.AddDocker(settings);

                if (data.IsModularDesign)
                {
                    WebProjectHelper.AddModulesToDockerfile(settings,
                        data.CustomModules
                            .Where(mi => mi.ModuleType == ModuleType.Web)
                            .Select(mi => mi.Name));
                }
            }
        }

        private static void AddModules()
        {
            var settings = ProjectSettings.Instance;
            var data = settings.ProjectTemplateData;

            if (data.CustomModules.Count == 0)
            {
                return;
            }

            foreach (var module in data.CustomModules)
            {
                if (module.ModuleType == ModuleType.Web)
                {
                    WebProjectHelper.AddModule(settings, module.Name);
                }
            }
        }

        private static class WebProjectHelper
        {
            internal static void AddDatabaseModule(ProjectSettings settings)
            {
                string token, connectionString;
                var data = settings.ProjectTemplateData;

                if (data.WebDatabaseInfrastructureType == WebDatabaseInfrastructureType.None)
                {
                    return;
                }

                AddModule(settings, AppConstants.DataAccessWebModule, AppConstants.WebDbTemplateName);
                var dbFileName = ConnectionStringHelper.GetWebDbFileName(settings);
                var projectFile = AppConstants.GetWebDatabaseProjectFilePath(settings);
                var jsonConfigFile = AppConstants.GetWebApiProjectFilePath(settings, AppConstants.AppSettingsJsonFile);

                switch (data.WebDatabaseInfrastructureType)
                {
                    case WebDatabaseInfrastructureType.MsSql:
                        token = AppConstants.MsSqlToken;
                        connectionString = ConnectionStringHelper.GetMsSqlConnectionString(dbFileName);
                        break;

                    case WebDatabaseInfrastructureType.MySql:
                        token = AppConstants.MySqlToken;
                        connectionString = ConnectionStringHelper.GetMySqlConnectionString(dbFileName);
                        break;

                    case WebDatabaseInfrastructureType.PostgreSql:
                        token = AppConstants.PostgreToken;
                        connectionString = ConnectionStringHelper.GetPostgreConnectionString(dbFileName);
                        break;

                    case WebDatabaseInfrastructureType.SqLite:
                        token = AppConstants.SqLiteToken;
                        connectionString = ConnectionStringHelper.GetSqLiteConnectionString(dbFileName);
                        break;

                    default: throw new NotSupportedException();
                }

                FileHelper.UncommentXmlLine(projectFile, token);
                FileHelper.ReplaceString(jsonConfigFile, AppConstants.ConnectionStringToken, connectionString);
            }

            internal static void AddWebProject(ProjectSettings settings)
            {
                var webProjectName = $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}";
                AddProject(settings, AppConstants.Web, AppConstants.WebTemplateName, webProjectName);

                var webProjecPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web, webProjectName,
                    $"{webProjectName}.csproj");

                var entitiesProjectName = $"{settings.SavedProjectName}.{AppConstants.Common}.{AppConstants.Entities}";

                FileHelper.InsertString(webProjecPath, "</PropertyGroup>",
                    "\n  <ItemGroup>\n    <ProjectReference Include=\"..\\..\\Common\\" + entitiesProjectName + "\\"
                    + entitiesProjectName + ".csproj\" />\n  </ItemGroup>\n\n");

                var controllerPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web, webProjectName,
                    "Controllers", "WeatherForecastApiController.cs");

                FileHelper.ReplaceString(controllerPath, AppConstants.NamespaceToken, settings.SavedProjectName);
            }

            internal static void AddSwagger(ProjectSettings settings)
            {
                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web,
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}",
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}.csproj");

                FileHelper.InsertString(path, "</PropertyGroup>", "  " + AppConstants.SwaggerInclude);

                path = path.Replace($"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}.csproj",
                    "Startup.cs");

                FileHelper.InsertString(path, "using System;", "using Microsoft.OpenApi.Models;\n");
                FileHelper.InsertString(path, "services.AddControllers();", "\t\t\tAddSwagger(services);\n");

                FileHelper.InsertStringAfter(path, "app.UseDeveloperExceptionPage();", 1,
                    "\n\t\t\tConfigureSwagger(app);\n");

                FileHelper.InsertStringAfter(path, "endpoints.MapControllers();", 2,
                    $"\t\t{AppConstants.SwaggerMethods.Replace(AppConstants.ProjectNameToken, $"\"{settings.SavedProjectName}\"")}\n");
            }

            internal static void AddDocker(ProjectSettings settings)
            {
                var dockerTemplatePath =
                    settings.Solution.GetProjectTemplate($"{AppConstants.DockerTemplateName}.zip", "Yaml");

                settings.Solution.AddFromTemplate(dockerTemplatePath, settings.SavedPath, AppConstants.DockerCompose);

                ConfigureProjectForDocker(settings);
                EditDockerFiles(settings);
            }

            private static void ConfigureProjectForDocker(ProjectSettings settings)
            {
                var webProjectPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web,
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}");

                FileHelper.InsertString(Path.Combine(webProjectPath,
                        $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}.csproj"),
                    "<TargetFramework>netcoreapp3.1</TargetFramework>", AppConstants.DockerPropertyGroup);

                FileHelper.InsertString(Path.Combine(webProjectPath,
                        $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}.csproj"),
                    "</PropertyGroup>", AppConstants.DockerInclude);

                FileHelper.InsertStringAfter(Path.Combine(webProjectPath, "Properties", "launchSettings.json"),
                    "\"ASPNETCORE_ENVIRONMENT\": \"Development\"", 2, AppConstants.DockerLaunchSettings + "\n");

                var stream = File.Create(Path.Combine(webProjectPath, "Dockerfile"));
                stream.Close();

                FileHelper.ReplaceText("SenticodeTemplate.TemplateFiles.Dockerfile",
                    Path.Combine(webProjectPath, "Dockerfile"));
            }

            private static void EditDockerFiles(ProjectSettings settings)
            {
                var webProjectName = $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}";

                FileHelper.ReplaceString(Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web,
                    webProjectName, "Dockerfile"), AppConstants.ProjectNameToken, webProjectName);

                FileHelper.ReplaceString(Path.Combine(settings.SavedPath, "docker-compose.yml"),
                    AppConstants.ProjectNameToken, webProjectName.ToLower());

                FileHelper.ReplaceString(Path.Combine(settings.SavedPath, "docker-compose.override.yml"),
                    AppConstants.ProjectNameToken, webProjectName.ToLower());

                FileHelper.ReplaceString(Path.Combine(settings.SavedPath, "docker-compose.yml"),
                    AppConstants.ProjectPathToken, webProjectName);
            }

            internal static void AddModuleAggregator(ProjectSettings settings)
            {
                var moduleAggregatorName =
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.ModuleAggregator}";

                var webProjectName = $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}";

                AddProject(settings, AppConstants.Web, AppConstants.WebModuleAggregatorTemplateName,
                    moduleAggregatorName);

                var csPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web, webProjectName,
                    $"{webProjectName}.csproj");

                FileHelper.InsertString(csPath, "</PropertyGroup>",
                    "\n  <ItemGroup>\n    <ProjectReference Include=\"..\\" + moduleAggregatorName + "\\"
                    + moduleAggregatorName + ".csproj\" />\n  </ItemGroup>\n\n");

                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web, webProjectName,
                    "Startup.cs");

                FileHelper.InsertStringAfter(path, "public void ConfigureServices(IServiceCollection services", 1,
                    "\t\t\tModuleAggregator.ModuleAggregator.Instance.Initialize(services);\n");

                AddWebCommonProject(settings);
            }

            private static void AddWebCommonProject(ProjectSettings settings)
            {
                var webCommonProjectName =
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Common}.{AppConstants.Interfaces}";

                AddProject(settings, AppConstants.Web, AppConstants.Common,
                    AppConstants.WebCommonTemplateName, webCommonProjectName);

                var webCommonProject = ProjectHelper.GetProjectByName(settings.Solution, webCommonProjectName,
                    AppConstants.Common, AppConstants.Web);

                var moduleAggregatorName =
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.ModuleAggregator}";

                var moduleAggregatorProject =
                    ProjectHelper.GetProjectByName(settings.Solution, moduleAggregatorName, AppConstants.Web);

                ProjectHelper.AddProjectReference(moduleAggregatorProject, webCommonProject);

                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web, moduleAggregatorName,
                    $"{AppConstants.ModuleAggregator}.cs");

                FileHelper.ReplaceString(path, AppConstants.NamespaceToken, settings.SavedProjectName);
            }

            private static void AddProject(ProjectSettings settings, string folderName, string templateName,
                string projectName)
            {
                var templatePath = settings.Solution.GetProjectTemplate($"{templateName}.zip", "CSharp");
                var projectPath = Path.Combine(settings.SavedPath, AppConstants.Src, folderName, projectName);
                var solutionFolder = ProjectHelper.AddSolutionFolder(settings.Solution, folderName);
                ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
            }

            private static void AddProject(ProjectSettings settings, string baseFolderName,
                string folderName, string templateName, string projectName)
            {
                var templatePath = settings.Solution.GetProjectTemplate($"{templateName}.zip", "CSharp");

                var projectPath = Path.Combine(settings.SavedPath, AppConstants.Src, baseFolderName, folderName,
                    projectName);

                var solutionFolder = ProjectHelper.AddSolutionFolder(settings.Solution, baseFolderName);
                solutionFolder = ProjectHelper.AddSolutionFolder(solutionFolder, folderName);

                ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
            }

            internal static void AddModule(ProjectSettings settings, string moduleName,
                string template = AppConstants.WebModuleTemplateName)
            {
                AddProject(settings, AppConstants.Web, AppConstants.Modules,
                    template, $"{settings.SavedProjectName}.{moduleName}");

                var classname = ProjectHelper.RenameModuleInitializer(settings.SavedPath, settings.SavedProjectName,
                    AppConstants.Web, moduleName);

                var webCommonProjectName =
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Common}.{AppConstants.Interfaces}";

                var module = ProjectHelper.GetProjectByName(settings.Solution,
                    $"{settings.SavedProjectName}.{moduleName}",
                    AppConstants.Modules, AppConstants.Web);

                var moduleProjectPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web,
                    AppConstants.Modules, $"{settings.SavedProjectName}.{moduleName}", $"{settings.SavedProjectName}.{moduleName}.csproj");

                FileHelper.InsertString(moduleProjectPath, "</PropertyGroup>", "\n<ItemGroup>\n<ProjectReference Include = \"..\\..\\Common\\" +
                      webCommonProjectName + "\\" + $"{webCommonProjectName}.csproj\"/>\n</ItemGroup>\n");

                var path = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web, AppConstants.Modules,
                    $"{settings.SavedProjectName}.{moduleName}", $"{classname}.cs");

                FileHelper.InsertString(path, "using Microsoft.Extensions.DependencyInjection;",
                    $"using {webCommonProjectName}.Core;\n");

                var moduleAggregatorName =
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.ModuleAggregator}";

                var moduleAggregatorProject =
                    ProjectHelper.GetProjectByName(settings.Solution, moduleAggregatorName, AppConstants.Web);

                ProjectHelper.AddProjectReference(moduleAggregatorProject, module);

                var maPath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web,
                    moduleAggregatorName, $"{AppConstants.ModuleAggregator}.cs");

                FileHelper.InsertString(maPath, "using Microsoft.Extensions.DependencyInjection;",
                    $"using {settings.SavedProjectName}.{moduleName};\n");

                FileHelper.InsertStringAfter(maPath,
                    "public IServiceCollection Initialize(IServiceCollection services)", 1,
                    $"\t\t\t{classname}.Instance.Initialize(services);\n");
            }

            internal static void AddModulesToDockerfile(ProjectSettings settings, IEnumerable<string> modules)
            {
                var dockerfilePath = Path.Combine(settings.SavedPath, AppConstants.Src, AppConstants.Web,
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Api}", "Dockerfile");

                foreach (var module in modules)
                {
                    var str =
                        $"COPY [\"{AppConstants.Src}/{AppConstants.Web}/{AppConstants.Modules}/{settings.SavedProjectName}.{module}/{settings.SavedProjectName}.{module}.csproj\", "
                        + $"\"{AppConstants.Src}/{AppConstants.Web}/{AppConstants.Modules}/{settings.SavedProjectName}.{module}/\"]\n";

                    FileHelper.InsertString(dockerfilePath, "WORKDIR /src", str);
                }

                var moduleAggregatorName =
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.ModuleAggregator}";

                var moduleAggregatorStr =
                    $"COPY [\"{AppConstants.Src}/{AppConstants.Web}/{moduleAggregatorName}/{moduleAggregatorName}.csproj\", "
                    + $"\"{AppConstants.Src}/{AppConstants.Web}/{moduleAggregatorName}/\"]\n";

                FileHelper.InsertString(dockerfilePath, "WORKDIR /src", moduleAggregatorStr);

                var webCommonProjectName =
                    $"{settings.SavedProjectName}.{AppConstants.Web}.{AppConstants.Common}.{AppConstants.Interfaces}";

                var webCommonStr =
                    $"COPY [\"{AppConstants.Src}/{AppConstants.Web}/{AppConstants.Common}/{webCommonProjectName}/{webCommonProjectName}.csproj\", "
                    + $"\"{AppConstants.Src}/{AppConstants.Web}/{AppConstants.Common}/{webCommonProjectName}/\"]\n";

                FileHelper.InsertString(dockerfilePath, "WORKDIR /src", webCommonStr);
            }
        }

        #region singleton

        private WebProjectManager()
        {
        }

        public static WebProjectManager Instance { get; } = new WebProjectManager();

        #endregion
    }
}