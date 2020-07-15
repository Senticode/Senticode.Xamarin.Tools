using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Senticode.Xamarin.Tools.Core.Configuration.Extensions;

namespace Senticode.Xamarin.Tools.Core.Configuration
{
    /// <summary>
    ///     Provides single access to configuration file for xamarin forms application.
    /// </summary>
    public static class ConfigurationManager
    {
        private const string ROOT_ELEMENT = "configuration";
        public static IReadOnlyDictionary<string, string> AppSettings { get; private set; }
        public static IReadOnlyDictionary<string, ConnectionStringSettings> ConnectionStrings { get; private set; }

        /// <summary>
        ///     Initialize AppSettings and ConnectionStrings collections from a configuration file read as a stream.
        /// </summary>
        /// <param name="configurationFile"></param>
        /// <example>
        ///     This sample shows how to call the <see cref="Initialize" /> method.
        ///     Load application configuration:
        ///     <code>
        ///     using (var config = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream(CONFIG_FILE))
        ///     {
        ///         ConfigurationManager.Initialize(config);
        ///     }
        ///     </code>
        /// </example>
        public static void Initialize(Stream configurationFile)
        {
            if (AppSettings != null)
            {
                throw new TypeInitializationException(nameof(ConfigurationManager),
                    new InvalidOperationException("Initialize must be called once in program"));
            }

            using (var reader = new StreamReader(configurationFile))
            {
                var doc = XDocument.Parse(reader.ReadToEnd());
                var config = LoadSection<Configuration>(doc);
                AppSettings = config.Settings.ToDictionary(x => x.Key, x => x.Value);
                ConnectionStrings = config.ConnectionStrings.ToDictionary(x => x.Name, x => x);
            }
        }

        /// <summary>
        ///     Retrieves a specified configuration section for the current application's default configuration.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSection(string key)
        {
            return AppSettings[key];
        }

        /// <summary>
        ///     Retrieves a specified connection string for the current application's default configuration.
        /// </summary>
        /// <param name="name"> The connection string name has the default value "DefaultConnection" </param>
        /// <returns></returns>
        public static string GetConnectionString(string name = "DefaultConnection")
        {
            return ConnectionStrings[name].ConnectionString;
        }

        private static T LoadSection<T>(XDocument doc)
        {
            // Load custom sections
            // Skip custom element if root is configuration
            var sectionName = typeof(T).Name.ToLower() == ROOT_ELEMENT ? null : typeof(T).Name.Camelize();
            var section = GetXElementSection(sectionName, doc);

            using (var stream = new MemoryStream())
            {
                var element = section;
                element.Save(stream);
                stream.Position = 0;

                var serializer = new XmlSerializer(typeof(T));
                var result = serializer.Deserialize(stream);
                return (T) result;
            }
        }

        private static XElement GetXElementSection(string sectionName, XDocument doc)
        {
            var section = string.IsNullOrEmpty(sectionName)
                ? doc.Element(ROOT_ELEMENT)
                : doc?.Element(ROOT_ELEMENT)?.Element(sectionName);

            return section;
        }
    }
}