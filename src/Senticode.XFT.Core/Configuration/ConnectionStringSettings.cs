using System.Xml.Serialization;

namespace Senticode.Xamarin.Tools.Core.Configuration
{
    [XmlRoot("add")]
    public class ConnectionStringSettings
    {
        [XmlAttribute("name")] public string Name { get; set; }
        [XmlAttribute("connectionString")] public string ConnectionString { get; set; }
        [XmlAttribute("providerName")] public string ProviderName { get; set; }
    }
}