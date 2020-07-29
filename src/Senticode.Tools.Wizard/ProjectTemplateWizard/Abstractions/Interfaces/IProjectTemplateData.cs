using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectTemplateWizard.Abstractions.Interfaces
{
    public enum ProjectTemplateType
    {
        [Display(
            Name = "Master-Detail",
            Description =
                "A project template for creating a Xamarin.Forms app that uses a ...TODO" /*todo: write a description*/)]
        MasterDetail,

        [Display(
            Name = "Blank",
            Description = "TODO... Blank template description" /*todo: write a description*/)]
        Blank,

        [Display(
            Name = "Shell",
            Description = "TODO... Shell template description" /*todo: write a description*/)]
        Shell
    }

    public enum PlatformType
    {
        [Display(Name = "Android")] Android,
        [Display(Name = "iOS")] Ios,
        [Display(Name = "UWP")] Uwp,
        [Display(Name = "WPF")] Wpf,
        [Display(Name = "GTK#")] GtkSharp
    }

    public enum XamarinDatabaseInfrastructureType
    {
        [Display(Name = "-")] None,
        [Display(Name = "SQLite")] SqLite
    }

    public enum WebDatabaseInfrastructureType
    {
        [Display(Name = "-")] None,
        [Display(Name = "MS SQL")] MsSql,
        [Display(Name = "PostgreSQL")] PostgreSql,
        [Display(Name = "MySQL")] MySql,
        [Display(Name = "SQLite")] SqLite
    }

    public enum ModuleType
    {
        [Display(Name = "Xamarin")] Xamarin,
        [Display(Name = "Web")] Web
    }

    public readonly struct ModuleInfo
    {
        public ModuleInfo(string name, ModuleType moduleType)
        {
            Name = name;
            ModuleType = moduleType;
        }

        public string Name { get; }
        public ModuleType ModuleType { get; }
    }

    public interface IProjectTemplateData
    {
        /// <summary>
        ///     Select template.
        /// </summary>
        ProjectTemplateType ProjectTemplateType { get; }

        /// <summary>
        ///     Manually added modules.
        /// </summary>
        IReadOnlyList<ModuleInfo> CustomModules { get; }

        #region Architecture Pattern

        /// <summary>
        ///     Select architecture pattern (basic or modular design).
        /// </summary>
        bool IsModularDesign { get; }

        #endregion

        #region Platform

        /// <summary>
        ///     Selected platforms.
        /// </summary>
        IReadOnlyList<PlatformType> SelectedPlatforms { get; }

        #endregion

        #region Unit Tests

        /// <summary>
        ///     Include nUnit with moq.
        /// </summary>
        bool IsNUnitIncluded { get; }

        #endregion

        #region Assets

        string AppIconPath { get; }
        string SplashScreenImagePath { get; }
        string SplashScreenBackgroundColor { get; }
        string AppIconBackgroundColor { get; }

        #endregion

        #region Web Backend

        /// <summary>
        ///     Include asp.net core web api project.
        /// </summary>
        bool IsWebBackendIncluded { get; }

        /// <summary>
        ///     Include docker compose project.
        /// </summary>
        bool IsDockerComposeIncluded { get; }

        /// <summary>
        ///     Include signalR infrastructure.
        /// </summary>
        bool IsSignalRIncluded { get; }

        /// <summary>
        ///     Include swagger.
        /// </summary>
        bool IsSwaggerIncluded { get; }

        /// <summary>
        ///     Include microsoft identity server.
        /// </summary>
        bool IsIdentityServerIncluded { get; }

        #endregion

        #region Database

        /// <summary>
        ///     Xamarin app database infrastructure.
        /// </summary>
        XamarinDatabaseInfrastructureType XamarinDatabaseInfrastructureType { get; }

        /// <summary>
        ///     Web backend database infrastructure.
        /// </summary>
        WebDatabaseInfrastructureType WebDatabaseInfrastructureType { get; }

        #endregion

        #region Others

        /// <summary>
        ///     Include readme.
        /// </summary>
        bool IsReadmeIncluded { get; }

        /// <summary>
        ///     Integrate versioning system.
        /// </summary>
        bool IsVersioningSystemIncluded { get; }

        /// <summary>
        ///     Include licenses info page.
        /// </summary>
        bool IsLicensesInfoPageIncluded { get; }

        #endregion
    }
}