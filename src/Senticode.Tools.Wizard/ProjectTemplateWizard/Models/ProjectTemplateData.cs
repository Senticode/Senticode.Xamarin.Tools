using System.Collections.Generic;
using ProjectTemplateWizard.Abstractions.Interfaces;

namespace ProjectTemplateWizard.Models
{
    internal class ProjectTemplateData : IProjectTemplateData
    {
        public string AppIconPath { get; set; }
        public string SplashScreenImagePath { get; set; }
        public string SplashScreenBackgroundColor { get; set; }
        public string AppIconBackgroundColor { get; set; }
        public ProjectTemplateType ProjectTemplateType { get; set; }
        public IReadOnlyList<ModuleInfo> CustomModules { get; set; }
        public bool IsModularDesign { get; set; }
        public IReadOnlyList<PlatformType> SelectedPlatforms { get; set; }
        public bool IsNUnitIncluded { get; set; }
        public bool IsWebBackendIncluded { get; set; }
        public bool IsDockerComposeIncluded { get; set; }
        public bool IsSignalRIncluded { get; set; }
        public bool IsSwaggerIncluded { get; set; }
        public bool IsIdentityServerIncluded { get; set; }
        public XamarinDatabaseInfrastructureType XamarinDatabaseInfrastructureType { get; set; }
        public WebDatabaseInfrastructureType WebDatabaseInfrastructureType { get; set; }
        public bool IsReadmeIncluded { get; set; }
        public bool IsVersioningSystemIncluded { get; set; }
        public bool IsLicensesInfoPageIncluded { get; set; }
    }
}