using System.Xml.Serialization;

namespace Senticode.Xamarin.Tools.Core.Configuration
{
    [XmlRoot("add")]
    public class Setting
    {
        [XmlAttribute("key")] public string Key { get; set; }

        [XmlAttribute("value")] public string Value { get; set; }
    }
}