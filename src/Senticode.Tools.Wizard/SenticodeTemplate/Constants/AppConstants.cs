namespace SenticodeTemplate.Constants
{
    internal static partial class AppConstants
    {
        public static readonly string ModuleAggregator = nameof(ModuleAggregator);
        public static readonly string ModuleInitializer = nameof(ModuleInitializer);
        public static readonly string AppInitializer = nameof(AppInitializer);

        #region Files

        public const string DbContextFile = "DatabaseContext.cs";
        public const string AppSettingsJsonFile = "appsettings.json";

        #endregion

        #region Naming

        public static readonly string Core = nameof(Core);
        public static readonly string Tools = nameof(Tools);
        public static readonly string Startup = nameof(Startup);
        public static readonly string Modules = nameof(Modules);
        public static readonly string Mobile = nameof(Mobile);
        public static readonly string Interfaces = nameof(Interfaces);
        public static readonly string Web = nameof(Web);
        public static readonly string Common = nameof(Common);
        public static readonly string Tests = nameof(Tests);
        public static readonly string Uwp = "UWP";
        public static readonly string Ios = "iOS";
        public static readonly string Android = "Android";
        public static readonly string Wpf = "WPF";
        public static readonly string Gtk = "GTK";
        public static readonly string Src = "src";
        public static readonly string Api = nameof(Api);
        public static readonly string DockerCompose = "docker-compose";
        public static readonly string WebClientModule = nameof(WebClientModule);
        public static readonly string DataAccessXamarinModule = nameof(DataAccessXamarinModule);
        public static readonly string DataAccessWebModule = nameof(DataAccessWebModule);
        public static readonly string Entities = "Entities";
        public static readonly string Infrastructure = "Infrastructure";
        public const string Sln = "sln";

        #endregion

        #region Project Templates

        public const string XamarinCoreTemplateName = "Template.MasterDetail";
        public const string AndroidTemplateName = "Template.Android";
        public const string IosTemplateName = "Template.iOS";
        public const string UwpTemplateName = "Template.UWP";
        public const string MobileModuleAggregatorTemplateName = "Template.Mobile.ModuleAggregator";
        public const string WebModuleAggregatorTemplateName = "Template.WEb.ModuleAggregator";
        public const string MobileModuleTemplateName = "Template.Module.Xamarin";
        public const string WebModuleTemplateName = "Template.Module.Web";
        public const string DockerTemplateName = "docker-compose";
        public const string WebTemplateName = "Template.Web.Api";
        public const string WpfTemplateName = "Template.Wpf";
        public const string GtkTemplateName = "Template.GtkSharp";
        public const string WebCommonTemplateName = "Template.Web.Common.Interfaces";
        public const string XamarinDbTemplateName = "Template.Db.Xamarin";
        public const string WebDbTemplateName = "Template.Db.Web";
        public const string CommonEntitiesTemplateName = "Template.Common.Entities";
        public const string WebInfrastructureTemplateName = "Template.Common.Web.Infrastructure";
        public const string MobileInterfacesTemplateName = "Template.Mobile.Interfaces";
        public const string WebClientTemplateName = "Template.WebClientModule";
        public const string TestProjectTemplateName = "Template.NUnitTests";
        public const string BlankTemplateName = "Template.Blank";
        public const string ShellTemplateName = "Template.Shell"; //TODO create template zip

        #endregion

        #region Code

        public const string SwaggerMethods = @"private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c => c.SwaggerDoc(""v1"", new OpenApiInfo { Title = $projectname$, Version = ""V1"" }));
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint(""/swagger/v1/swagger.json"", ""post API V1""));
        }";

        public const string SwaggerInclude = @"
        <ItemGroup>
            <PackageReference Include=""Swashbuckle.AspNetCore"" Version=""5.5.1"" />
        </ItemGroup>";

        public const string DockerInclude = @"
        <ItemGroup>
            <PackageReference Include=""Microsoft.VisualStudio.Azure.Containers.Tools.Targets"" Version=""1.9.10"" />
        </ItemGroup>";

        public const string DockerPropertyGroup = @"
        <DockerComposeProjectPath>..\docker-compose.dcproj</DockerComposeProjectPath>
        <UserSecretsId>db0dad7d-c6f9-4960-bd41-63a70cd7d228</UserSecretsId>
        <DockerDefaultTargetOS>Linux</DockerDefaultTargetOS>";

        public const string DockerLaunchSettings = @"     
    ""Docker"": {
        ""commandName"": ""Docker"",
        ""launchBrowser"": true,
        ""launchUrl"": ""{Scheme}://{ServiceHost}:{ServicePort}/weatherforecastapi"",
        ""publishAllPorts"": true,
        ""useSSL"": true
    },";

        public const string InitializeModelControllerMethod = "private async void InitializeModelController()";

        public const string InitializeModelController = @"
            var webService = Container.Resolve<IWeatherWebService>();
            var result = await webService.GetAllAsync();
            if (result.IsSuccessful)
            {
                var modelController = Container.Resolve<ModelController>();
                await modelController.ReplaceAllObjectsAsync(result.Object);
            }";

        public const string NavigateToLicenses = @"
                {
                    MenuKind.Licenses,
                    () => new ViewViewModelPair(container.Resolve<LicensesInfoMenu>(),
                    container.Resolve<LicensesMenuViewModel>())
                },";

        public const string LicensesMenuItem =
            "<menuElements:MenuItemView Grid.Row=\"2\" BindingContext=\"{Binding LicenseInfo}\" />";

        public const string LicenseInfo = @"
            LicenseInfo =
                new ActionViewModel(ResourceKeys.Licenses)
                {
                    Command = Container.Resolve<NavigateToMenuCommand>(),
                    Parameter = MenuKind.Licenses
                };";

        public const string DatabaseLicenses = @"
        <LicenseInfoViewModel>
            <Name>Microsoft.EntityFrameworkCore.Sqlite</Name>
            <Version>2.2.6</Version>
            <Uri>https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/</Uri>
            <UriText>nuget</UriText>
            <LicenseType>Apache License 2.0</LicenseType>
            <LicenseUri>https://licenses.nuget.org/Apache-2.0</LicenseUri>
        </LicenseInfoViewModel>
        <LicenseInfoViewModel>
            <Name>Microsoft.EntityFrameworkCore</Name>
            <Version>3.1.5</Version>
            <Uri>https://www.nuget.org/packages/Microsoft.EntityFrameworkCore</Uri>
            <UriText>nuget</UriText>
            <LicenseType>Apache License 2.0</LicenseType>
            <LicenseUri>https://licenses.nuget.org/Apache-2.0</LicenseUri>
        </LicenseInfoViewModel>
        <LicenseInfoViewModel>
            <Name>Senticode.Database.Tools</Name>
            <Version>1.0.0.14</Version>
            <Uri>https://www.nuget.org/packages/Senticode.Database.Tools</Uri>
            <UriText>nuget</UriText>
            <LicenseType>MIT License</LicenseType>
            <LicenseUri>https://github.com/Senticode/Senticode.Xamarin.UI/blob/dev/LICENSE</LicenseUri>
        </LicenseInfoViewModel>";

        public const string WebLicense = @"
        <LicenseInfoViewModel>
            <Name>Newtonsoft.Json</Name>
            <Version>12.0.3</Version>
            <Uri>https://www.nuget.org/packages/Newtonsoft.Json</Uri>
            <UriText>nuget</UriText>
            <LicenseType>MIT License</LicenseType>
            <LicenseUri>https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md</LicenseUri>
        </LicenseInfoViewModel>";

        public const string AssemblyInfoLink = @"
    <PropertyGroup>    
        <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    </PropertyGroup>

    <ItemGroup>
        <Compile Include=""{folders}sln\SharedAssemblyInfo.cs"" Link=""Properties\SharedAssemblyInfo.cs"" />
    </ItemGroup>";

        #endregion
    }
}