using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SenticodeTemplate.Services.Helpers
{
    internal static partial class FileHelper
    {
        public static readonly string CapturingGroup = nameof(CapturingGroup);

        private static Regex GetCsFileCommentRegex(string token) =>
            new Regex($@"\/\*{token}\*\/\s*\/\*(?<{CapturingGroup}>[\s\S]*)\*\/");

        private static Regex GetXmlFileCommentRegex(string token) =>
            new Regex($@"<!--{token}-->\s*<!--(?<{CapturingGroup}>[\s\S]*)-->");

        private static void Uncomment(string path, string token, Func<string, Regex> getRegex)
        {
            var content = File.ReadAllText(path);
            var regex = getRegex(token);
            content = regex.Replace(content, x => x.Groups[CapturingGroup].Value);
            File.WriteAllText(path, content);
        }

        public static void UncommentXml(string filePath, string token) =>
            Uncomment(filePath, token, GetXmlFileCommentRegex);

        public static void UncommentCs(string filePath, string token) =>
            Uncomment(filePath, token, GetCsFileCommentRegex);
    }
}