using Senticode.Xamarin.Tools.Core;
using Unity;
using Xamarin.Forms.Xaml;

namespace _template.MasterDetail.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
            ServiceLocator.Container.RegisterInstance(this);
        }
    }
}