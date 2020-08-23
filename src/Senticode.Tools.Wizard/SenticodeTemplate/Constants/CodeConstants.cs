namespace SenticodeTemplate.Constants
{
    internal static class CodeConstants
    {
        public const string SwaggerMethods = @"
        private void AddSwagger(IServiceCollection services)
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
    }
}