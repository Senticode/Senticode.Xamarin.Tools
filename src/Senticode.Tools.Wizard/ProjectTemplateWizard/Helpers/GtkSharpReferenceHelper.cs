using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ProjectTemplateWizard.Helpers
{
    internal static class GtkSharpReferenceHelper
    {
        private const string ErrorBoxCaption = "Referred path lacks Gtk# files";
        private const string GtkDownloadPageLinkMessage = "Go to www.mono-project.com and Donload Gtk#?";
        private const string GtkDownloadPageLink = "https://www.mono-project.com/download/stable";

        private static readonly string GtkSharpDirectory =
            $@"C:\{GetProgramFilesDirectory()}\GtkSharp\2.12\lib\gtk-sharp-2.0";

        private static readonly string[] GdkSharkRequiredLibs =
        {
            "atk-sharp.dll",
            "gdk-sharp.dll",
            "glade-sharp.dll",
            "glib-sharp.dll",
            "gtk-dotnet.dll",
            "gtk-sharp.dll"
        };

        private static string GetProgramFilesDirectory() =>
            Environment.Is64BitOperatingSystem ? "Program Files (x86)" : "Program Files";

        public static void CheckAvailability()
        {
            StringBuilder errorMessage = null;
            var directory = new DirectoryInfo(GtkSharpDirectory);

            if (directory.Exists)
            {
                var filesInDirectory = directory
                    .GetFiles()
                    .ToDictionary(x => x.Name, y => y);

                var missingLibs = GdkSharkRequiredLibs
                    .Where(lib => !filesInDirectory.ContainsKey(lib))
                    .ToArray();

                if (missingLibs.Length > 0)
                {
                    errorMessage = new StringBuilder();
                    errorMessage.AppendLine($"Files are missing on path {GtkSharpDirectory}");

                    foreach (var lib in missingLibs)
                    {
                        errorMessage.AppendLine(lib);
                    }
                }
            }
            else
            {
                errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Directory {GtkSharpDirectory} not found.");
            }

            if (errorMessage == null)
            {
                return;
            }

            errorMessage.AppendLine();
            errorMessage.Append(GtkDownloadPageLinkMessage);
            var messageBoxResult = MessageBox.Show(errorMessage.ToString(), ErrorBoxCaption, MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Process.Start(GtkDownloadPageLink);
            }
        }
    }
}