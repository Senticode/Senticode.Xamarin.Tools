using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Template.MasterDetail.Views;
using Unity;

namespace Template.MasterDetail
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