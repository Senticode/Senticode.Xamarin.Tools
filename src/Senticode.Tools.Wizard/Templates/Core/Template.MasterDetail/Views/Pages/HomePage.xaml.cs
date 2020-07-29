using Senticode.Xamarin.Tools.Core;
using Xamarin.Forms.Xaml;
using Unity;

namespace Template.MasterDetail.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
            ServiceLocator.Container.RegisterInstance<HomePage>(this);
        }
    }
}