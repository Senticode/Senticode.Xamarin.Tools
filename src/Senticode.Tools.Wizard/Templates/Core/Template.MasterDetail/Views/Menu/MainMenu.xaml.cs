using Senticode.Xamarin.Tools.Core;
using Unity;
using Xamarin.Forms.Xaml;

namespace _template.MasterDetail.Views.Menu
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