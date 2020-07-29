using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Template.MasterDetail.Commands.Navigation;
using Template.MasterDetail.Resources;
using Template.MasterDetail.ViewModels.Abstractions;
using Unity;
using Xamarin.Forms;

namespace Template.MasterDetail.ViewModels.Menu
{
    internal class AboutMenuViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        public AboutMenuViewModel(IUnityContainer container)
        {
            container.RegisterInstance(this);


            Title = ResourceKeys.About;

        }
    }
}