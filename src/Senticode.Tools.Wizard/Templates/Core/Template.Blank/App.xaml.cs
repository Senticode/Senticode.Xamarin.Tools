using _template.Blank.Views;
using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace _template.Blank
{
    public partial class App : XamarinApplicationBase<AppSettings, AppCommands, AppLifeTimeManager>
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