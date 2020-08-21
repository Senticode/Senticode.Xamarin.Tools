using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectTemplateWizard.Abstractions.Interfaces;
using SenticodeTemplate.Constants;
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

            AddWebProject(settings);
            AddSwagger(settings);
            AddDocker(settings);

            if (data.IsModularDesign)
            {
                AddModuleAggregator(settings);
                AddDatabaseModule(settings);
                AddCustomModules();
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
                if (module.ModuleType == ModuleType.Web)
                {
                    AddModule(settings, module.Name);
                }
            }
        }

        private static void AddDatabaseModule(ProjectSettings settings)
        {
            string token, connectionString;
            var data = settings.ProjectTemplateData;

            if (data.WebDatabaseInfrastructureType == WebDatabaseInfrastructureType.None)
            {
                return;
            }

            AddModule(settings, StringLiterals.DataAccessWebModule, TemplateProjectNames.WebDb);
            var dbFileName = ConnectionStringHelper.GetWebDbFileName(settings);
            var projectFile = StringLiterals.GetWebDatabaseProjectFilePath(settings);
            var jsonConfigFile = StringLiterals.GetWebApiProjectFilePath(settings, FileNames.AppSettingsJson);

            switch (data.WebDatabaseInfrastructureType)
            {
                case WebDatabaseInfrastructureType.MsSql:
                    token = ReplacementTokens.MsSql;
                    connectionString = ConnectionStringHelper.GetMsSqlConnectionString(dbFileName);
                    break;

                case WebDatabaseInfrastructureType.MySql:
                    token = ReplacementTokens.MySql;
                    connectionString = ConnectionStringHelper.GetMySqlConnectionString(dbFileName);
                    break;

                case WebDatabaseInfrastructureType.PostgreSql:
                    token = ReplacementTokens.Postgre;
                    connectionString = ConnectionStringHelper.GetPostgreConnectionString(dbFileName);
                    break;

                case WebDatabaseInfrastructureType.SqLite:
                    token = ReplacementTokens.SqLite;
                    connectionString = ConnectionStringHelper.GetSqLiteConnectionString(dbFileName);
                    break;

                default: throw new NotSupportedException();
            }

            FileHelper.UncommentXml(projectFile, token);
            FileHelper.ReplaceString(jsonConfigFile, ReplacementTokens.ConnectionString, connectionString);
        }

        private static void AddWebProject(ProjectSettings settings)
        {
            var webProjectName = $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}";
            AddProject(settings, StringLiterals.Web, TemplateProjectNames.Web, webProjectName);

            var webProjectPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                webProjectName, $"{webProjectName}.{FileExtensions.CsProj}");

            var entitiesProjectName =
                $"{settings.SavedProjectName}.{StringLiterals.Common}.{StringLiterals.Entities}";

            FileHelper.InsertString(webProjectPath, StringLiterals.PropertyGroupTag,
                $"\n<ItemGroup>\n<ProjectReference Include=\"..\\..\\Common\\{entitiesProjectName}\\{entitiesProjectName}.{FileExtensions.CsProj}\"/>\n</ItemGroup>\n\n");

            var controllerPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                webProjectName, StringLiterals.Controllers, FileNames.WeatherForecastApiControllerCs);

            FileHelper.ReplaceString(controllerPath, ReplacementTokens.TemplateNamespace,
                settings.SavedProjectName);
        }

        private static void AddSwagger(ProjectSettings settings)
        {
            if (!settings.ProjectTemplateData.IsSwaggerIncluded)
            {
                return;
            }

            var webApiPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}",
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}.{FileExtensions.CsProj}");

            FileHelper.InsertString(webApiPath, StringLiterals.PropertyGroupTag, CodeConstants.SwaggerInclude);

            var startupPath = webApiPath.Replace(
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}.{FileExtensions.CsProj}",
                FileNames.StartupCs);

            FileHelper.InsertString(startupPath, "using System;", "using Microsoft.OpenApi.Models;\n");
            FileHelper.InsertString(startupPath, "services.AddControllers();", "\t\t\tAddSwagger(services);\n");

            FileHelper.InsertStringAfter(startupPath, "app.UseDeveloperExceptionPage();", 1,
                "\n\t\t\tConfigureSwagger(app);\n");

            FileHelper.InsertStringAfter(startupPath, "endpoints.MapControllers();", 2,
                $"\t\t{CodeConstants.SwaggerMethods.Replace(ReplacementTokens.ProjectName, $"\"{settings.SavedProjectName}\"")}\n");
        }

        private static void AddDocker(ProjectSettings settings)
        {
            var data = settings.ProjectTemplateData;

            if (!data.IsDockerComposeIncluded)
            {
                return;
            }

            var dockerTemplatePath =
                settings.Solution.GetProjectTemplate($"{TemplateProjectNames.Docker}.{FileExtensions.Zip}",
                    LanguageNames.Yaml);

            settings.Solution.AddFromTemplate(dockerTemplatePath, settings.SavedPath, StringLiterals.DockerCompose);
            ConfigureProjectForDocker(settings);
            EditDockerFiles(settings);

            if (data.IsModularDesign)
            {
                AddModulesToDockerfile(settings, data.CustomModules
                    .Where(mi => mi.ModuleType == ModuleType.Web)
                    .Select(mi => mi.Name));
            }
        }

        private static void ConfigureProjectForDocker(ProjectSettings settings)
        {
            var webProjectPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}");

            FileHelper.InsertString(Path.Combine(webProjectPath,
                    $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}.{FileExtensions.CsProj}"),
                "<TargetFramework>netcoreapp3.1</TargetFramework>", CodeConstants.DockerPropertyGroup);

            FileHelper.InsertString(Path.Combine(webProjectPath,
                    $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}.{FileExtensions.CsProj}"),
                StringLiterals.PropertyGroupTag, CodeConstants.DockerInclude);

            FileHelper.InsertStringAfter(
                Path.Combine(webProjectPath, StringLiterals.Properties, FileNames.LaunchSettingsJson),
                "\"ASPNETCORE_ENVIRONMENT\": \"Development\"", 2, $"{CodeConstants.DockerLaunchSettings}\n");

            using (File.Create(Path.Combine(webProjectPath, StringLiterals.Dockerfile)))
            {
            }

            FileHelper.ReplaceText(FileNames.SenticodeTemplateTemplateFilesDockerfile,
                Path.Combine(webProjectPath, StringLiterals.Dockerfile));
        }

        private static void EditDockerFiles(ProjectSettings settings)
        {
            var webProjectName = $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}";

            FileHelper.ReplaceString(Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                webProjectName, StringLiterals.Dockerfile), ReplacementTokens.ProjectName, webProjectName);

            FileHelper.ReplaceString(Path.Combine(settings.SavedPath, FileNames.DockerComposeYml),
                ReplacementTokens.ProjectName, webProjectName.ToLower());

            FileHelper.ReplaceString(Path.Combine(settings.SavedPath, FileNames.DockerComposeOverrideYml),
                ReplacementTokens.ProjectName, webProjectName.ToLower());

            FileHelper.ReplaceString(Path.Combine(settings.SavedPath, FileNames.DockerComposeYml),
                ReplacementTokens.ProjectPath, webProjectName);
        }

        private static void AddModuleAggregator(ProjectSettings settings)
        {
            var moduleAggregatorName =
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.ModuleAggregator}";

            var webProjectName = $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}";

            AddProject(settings, StringLiterals.Web, TemplateProjectNames.WebModuleAggregator,
                moduleAggregatorName);

            var csPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web, webProjectName,
                $"{webProjectName}.{FileExtensions.CsProj}");

            FileHelper.InsertString(csPath, StringLiterals.PropertyGroupTag,
                $"\n<ItemGroup>\n<ProjectReference Include=\"..\\{moduleAggregatorName}\\{moduleAggregatorName}.{FileExtensions.CsProj}\"/>\n</ItemGroup>\n\n");

            var path = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web, webProjectName,
                FileNames.StartupCs);

            FileHelper.InsertStringAfter(path, "public void ConfigureServices(IServiceCollection services", 1,
                "\t\t\tModuleAggregator.ModuleAggregator.Instance.Initialize(services);\n");

            AddWebCommonProject(settings);
        }

        private static void AddWebCommonProject(ProjectSettings settings)
        {
            var webCommonProjectName =
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Common}.{StringLiterals.Interfaces}";

            AddProject(settings, StringLiterals.Web, StringLiterals.Common,
                TemplateProjectNames.WebCommon, webCommonProjectName);

            var webCommonProject = ProjectHelper.GetProjectByName(settings.Solution, webCommonProjectName,
                StringLiterals.Common, StringLiterals.Web);

            var moduleAggregatorName =
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.ModuleAggregator}";

            var moduleAggregatorProject =
                ProjectHelper.GetProjectByName(settings.Solution, moduleAggregatorName, StringLiterals.Web);

            ProjectHelper.AddProjectReference(moduleAggregatorProject, webCommonProject);

            var path = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                moduleAggregatorName, $"{StringLiterals.ModuleAggregator}.{FileExtensions.Cs}");

            FileHelper.ReplaceString(path, ReplacementTokens.TemplateNamespace, settings.SavedProjectName);
        }

        private static void AddProject(ProjectSettings settings, string folderName, string templateName,
            string projectName)
        {
            var templatePath =
                settings.Solution.GetProjectTemplate($"{templateName}.{FileExtensions.Zip}", LanguageNames.CSharp);

            var projectPath = Path.Combine(settings.SavedPath, StringLiterals.Src, folderName, projectName);
            var solutionFolder = ProjectHelper.AddSolutionFolder(settings.Solution, folderName);

            ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
        }

        private static void AddProject(ProjectSettings settings, string baseFolderName,
            string folderName, string templateName, string projectName)
        {
            var templatePath =
                settings.Solution.GetProjectTemplate($"{templateName}.{FileExtensions.Zip}", LanguageNames.CSharp);

            var projectPath = Path.Combine(settings.SavedPath, StringLiterals.Src, baseFolderName, folderName,
                projectName);

            var solutionFolder = ProjectHelper.AddSolutionFolder(settings.Solution, baseFolderName);
            solutionFolder = ProjectHelper.AddSolutionFolder(solutionFolder, folderName);

            ProjectHelper.AddProjectToSolutionFolder(solutionFolder, templatePath, projectPath, projectName);
        }

        private static void AddModule(ProjectSettings settings, string moduleName,
            string template = TemplateProjectNames.WebModule)
        {
            AddProject(settings, StringLiterals.Web, StringLiterals.Modules,
                template, $"{settings.SavedProjectName}.{moduleName}");

            var classname = ProjectHelper.RenameModuleInitializer(settings.SavedPath, settings.SavedProjectName,
                StringLiterals.Web, moduleName);

            var webCommonProjectName =
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Common}.{StringLiterals.Interfaces}";

            var module = ProjectHelper.GetProjectByName(settings.Solution,
                $"{settings.SavedProjectName}.{moduleName}",
                StringLiterals.Modules, StringLiterals.Web);

            var moduleProjectPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                StringLiterals.Modules, $"{settings.SavedProjectName}.{moduleName}",
                $"{settings.SavedProjectName}.{moduleName}.{FileExtensions.CsProj}");

            FileHelper.InsertString(moduleProjectPath, StringLiterals.PropertyGroupTag,
                $"\n<ItemGroup>\n<ProjectReference Include=\"..\\..\\Common\\{webCommonProjectName}\\{webCommonProjectName}.{FileExtensions.CsProj}\"/>\n</ItemGroup>\n");

            var path = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                StringLiterals.Modules, $"{settings.SavedProjectName}.{moduleName}",
                $"{classname}.{FileExtensions.Cs}");

            FileHelper.InsertString(path, "using Microsoft.Extensions.DependencyInjection;",
                $"using {webCommonProjectName}.Core;\n");

            var moduleAggregatorName =
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.ModuleAggregator}";

            var moduleAggregatorProject =
                ProjectHelper.GetProjectByName(settings.Solution, moduleAggregatorName, StringLiterals.Web);

            ProjectHelper.AddProjectReference(moduleAggregatorProject, module);

            var maPath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                moduleAggregatorName, $"{StringLiterals.ModuleAggregator}.{FileExtensions.Cs}");

            FileHelper.InsertString(maPath, "using Microsoft.Extensions.DependencyInjection;",
                $"using {settings.SavedProjectName}.{moduleName};\n");

            FileHelper.InsertStringAfter(maPath,
                "public IServiceCollection Initialize(IServiceCollection services)", 1,
                $"\t\t\t{classname}.Instance.Initialize(services);\n");
        }

        private static void AddModulesToDockerfile(ProjectSettings settings, IEnumerable<string> modules)
        {
            var dockerfilePath = Path.Combine(settings.SavedPath, StringLiterals.Src, StringLiterals.Web,
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Api}", StringLiterals.Dockerfile);

            foreach (var module in modules)
            {
                var str =
                    $"COPY [\"{StringLiterals.Src}/{StringLiterals.Web}/{StringLiterals.Modules}/{settings.SavedProjectName}.{module}/{settings.SavedProjectName}.{module}.{FileExtensions.CsProj}\", \"{StringLiterals.Src}/{StringLiterals.Web}/{StringLiterals.Modules}/{settings.SavedProjectName}.{module}/\"]\n";

                FileHelper.InsertString(dockerfilePath, "WORKDIR /src", str);
            }

            var moduleAggregatorName =
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.ModuleAggregator}";

            var moduleAggregatorStr =
                $"COPY [\"{StringLiterals.Src}/{StringLiterals.Web}/{moduleAggregatorName}/{moduleAggregatorName}.{FileExtensions.CsProj}\", \"{StringLiterals.Src}/{StringLiterals.Web}/{moduleAggregatorName}/\"]\n";

            FileHelper.InsertString(dockerfilePath, "WORKDIR /src", moduleAggregatorStr);

            var webCommonProjectName =
                $"{settings.SavedProjectName}.{StringLiterals.Web}.{StringLiterals.Common}.{StringLiterals.Interfaces}";

            var webCommonStr =
                $"COPY [\"{StringLiterals.Src}/{StringLiterals.Web}/{StringLiterals.Common}/{webCommonProjectName}/{webCommonProjectName}.{FileExtensions.CsProj}\", \"{StringLiterals.Src}/{StringLiterals.Web}/{StringLiterals.Common}/{webCommonProjectName}/\"]\n";

            FileHelper.InsertString(dockerfilePath, "WORKDIR /src", webCommonStr);
        }

        #region singleton

        private WebProjectManager()
        {
        }

        public static WebProjectManager Instance { get; } = new WebProjectManager();

        #endregion
    }
}