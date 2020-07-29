using System.IO;
using System.Xml.Serialization;
using Unity;

namespace Template.Blank.Services.Data
{
    public class ExportImportService<T>
    {
        public ExportImportService(IUnityContainer container)
        {
            container.RegisterInstance<ExportImportService<T>>(this);
        }

        public T Import(string path)
        {
            var xml = File.ReadAllText(path);
            using (var reader = new StringReader(xml))
            {
                var config = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                return config;
            }
        }

        public T Import(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public void Export(string path, T config)
        {
            using (var writer = new StreamWriter(path))
            {
                new XmlSerializer(typeof(T)).Serialize(writer, config);
            }
        }
    }
}