using System.IO;
using System.Reflection;
using System.Text;

namespace SenticodeTemplate.Services.Helpers
{
    internal partial class FileHelper
    {
        public static void ReplaceString(string path, string oldValue, string newValue)
        {
            var text = File.ReadAllText(path);
            text = text.Replace(oldValue, newValue);
            File.WriteAllText(path, text);
        }

        public static void ReplaceText(string source, string destination)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(source);
            using var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            File.WriteAllText(destination, text);
        }

        public static void InsertString(string path, string position, string text)
        {
            var sb = new StringBuilder();

            using (var sr = new StreamReader(path))
            {
                string line;

                do
                {
                    line = sr.ReadLine();
                    sb.AppendLine(line);
                } while (string.IsNullOrEmpty(line) || !line.Contains(position));

                sb.Append(text);
                sb.Append(sr.ReadToEnd());
            }

            using (var sr = new StreamWriter(path))
            {
                sr.Write(sb.ToString());
            }
        }

        public static void InsertStringAfter(string path, string position, int linesNumber, string text)
        {
            var sb = new StringBuilder();

            using (var sr = new StreamReader(path))
            {
                string line;

                do
                {
                    line = sr.ReadLine();
                    sb.AppendLine(line);
                } while (string.IsNullOrEmpty(line) || !line.Contains(position));

                for (var i = 0; i < linesNumber; i++)
                {
                    line = sr.ReadLine();
                    sb.AppendLine(line);
                }

                sb.Append(text);
                sb.Append(sr.ReadToEnd());
            }

            using (var sr = new StreamWriter(path))
            {
                sr.Write(sb.ToString());
            }
        }

        public static string FindStringStartingWith(string path, string stringBeginning)
        {
            using var sr = new StreamReader(path);
            string line;

            do
            {
                line = sr.ReadLine();
            } while (string.IsNullOrEmpty(line) || !line.Trim().StartsWith(stringBeginning));

            return line;
        }
    }
}