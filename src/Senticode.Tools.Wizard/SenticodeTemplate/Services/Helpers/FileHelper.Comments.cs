using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SenticodeTemplate.Services.Helpers
{
    internal static partial class FileHelper
    {
        private const string CapturingGroup = "TargetLine";

        private static Regex GetCsFileCommentRegex(string token) =>
            new Regex($@"\/\*{token}\*\/\s*\/\*(?<{CapturingGroup}>.*)\*\/");

        private static Regex GetXmlFileCommentRegex(string token) =>
            new Regex($@"<!--{token}-->\s*<!--(?<{CapturingGroup}>.*)-->");

        public static void UncommentLine(string path, string token, Func<string, Regex> getRegex)
        {
            var content = File.ReadAllText(path);
            var regex = getRegex(token);
            content = regex.Replace(content, x => x.Groups[CapturingGroup].Value);
            File.WriteAllText(path, content);
        }

        public static void UncommentXmlLine(string filePath, string token) =>
            UncommentLine(filePath, token, GetXmlFileCommentRegex);

        public static void UncommentCsLine(string filePath, string token) =>
            UncommentLine(filePath, token, GetCsFileCommentRegex);
    }
}