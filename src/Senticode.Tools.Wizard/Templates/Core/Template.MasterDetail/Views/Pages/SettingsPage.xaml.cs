using Senticode.Xamarin.Tools.Core;
using Xamarin.Forms.Xaml;
using Unity;

namespace Template.MasterDetail.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            ServiceLocator.Container.RegisterInstance<SettingsPage>(this);
        }
    }
}