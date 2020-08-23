using System;

namespace SenticodeTemplate.Constants
{
    internal static class ReplacementTokens
    {
        [Obsolete("Use ModuleInitializer class name to replace")]
        public const string ModuleInitializer = "$moduleinitializer$";

        public const string TemplateNamespace = "_template";
        public const string ProjectName = "$projectname$";
        public const string ProjectPath = "$projectpath$";
        public const string SafeProjectName = "$safeprojectname$";
        public const string SolutionDirectory = "$solutiondirectory$";
        public const string ConnectionString = "_connectionstring_";
        public const string SqLite = "_sqlite_";
        public const string MySql = "_mysql_";
        public const string MsSql = "_mssql_";
        public const string Postgre = "_postgre_";
        public const string WebClientRegistration = "_webclientregistration_";
        public const string ConfigureSwagger = "_configureSwagger_";
        public const string AddSwagger = "_addSwagger_";
        public const string Licenses = "_licenses_";
        public const string LicensesInfo = "_licensesInfo_";
        public const string LicensesInfoProperty = "_licensesInfoProperty_";
    }
}