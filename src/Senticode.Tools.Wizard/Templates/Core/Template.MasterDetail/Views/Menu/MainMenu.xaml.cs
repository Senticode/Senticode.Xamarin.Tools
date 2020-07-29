using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Unity;
using Xamarin.Forms.Xaml;

namespace Template.MasterDetail.Views.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu
    {
        public MainMenu()
        {
            InitializeComponent();
            ServiceLocator.Container.RegisterInstance(this);
        }
    }
}