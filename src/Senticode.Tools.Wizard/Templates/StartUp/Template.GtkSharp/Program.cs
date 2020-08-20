using System;
using _template.MasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Application = Gtk.Application;

namespace _template.GtkSharp
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.Init();
            Forms.SetFlags("RadioButton_Experimental");
            Forms.Init();
            var app = new App(GtkSharpInitializer.Instance);
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("Template.GtkSharp");
            window.Show();
            Application.Run();
        }
    }
}