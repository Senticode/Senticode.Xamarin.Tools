using System.Xml.Serialization;

namespace Senticode.Xamarin.Tools.Core.Configuration
{
    [XmlRoot("add")]
    public class ConnectionStringSettings
    {
        [XmlAttribute("name")] public string Name { get; internal set; }
        [XmlAttribute("connectionString")] public string ConnectionString { get; internal set; }
        [XmlAttribute("providerName")] public string ProviderName { get; internal set; }
    }
}