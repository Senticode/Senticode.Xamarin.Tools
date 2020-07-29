using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Unity;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using System.Diagnostics;
using Template.Blank.Views;

namespace Template.Blank
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
