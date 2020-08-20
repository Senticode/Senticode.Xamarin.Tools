using System;
using System.IO;
using System.Xml.Serialization;
using Unity;

namespace _template.Blank.Services.Data
{
    public class ExportImportService<T>
    {
        public ExportImportService(IUnityContainer container)
        {
            container.RegisterInstance(this);
        }

        public T Import(string path)
        {
            var xml = File.ReadAllText(path);
            using (var reader = new StringReader(xml))
            {
                var config = (T) new XmlSerializer(typeof(T)).Deserialize(reader);
                return config;
            }
        }

        public T Import(Stream stream) => throw new NotImplementedException();

        public void Export(string path, T config)
        {
            using (var writer = new StreamWriter(path))
            {
                new XmlSerializer(typeof(T)).Serialize(writer, config);
            }
        }
    }
}