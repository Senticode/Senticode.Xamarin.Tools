using Senticode.Xamarin.Tools.Core;
using Unity;
using Xamarin.Forms.Xaml;

namespace _template.MasterDetail.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            ServiceLocator.Container.RegisterInstance(this);
        }
    }
}