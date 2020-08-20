using _template.MasterDetail.Views;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace _template.MasterDetail
{
    public partial class App
    {
        public App(IPlatformInitializer initializer) : base(initializer)
        {
            InitializeComponent();
            MainPage = new PreviewPage();
        }

        protected override void RegisterTypes(IUnityContainer container)
        {
            AppInitializer.Instance.Initialize(container);
        }
    }
}