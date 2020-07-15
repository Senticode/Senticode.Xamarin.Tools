using System.Collections.Generic;
using System.Xml.Serialization;

namespace Senticode.Xamarin.Tools.Core.Configuration
{
    [XmlRoot("configuration")]
    public class Configuration
    {
        public Configuration()
        {
            Settings = new List<Setting>();
        }

        [XmlArray("appSettings")]
        [XmlArrayItem(typeof(Setting), ElementName = "add")]
        public List<Setting> Settings { get; set; }

        [XmlArray("connectionStrings")]
        [XmlArrayItem(typeof(Setting), ElementName = "add")]
        public List<ConnectionStringSettings> ConnectionStrings { get; set; }
    }
}