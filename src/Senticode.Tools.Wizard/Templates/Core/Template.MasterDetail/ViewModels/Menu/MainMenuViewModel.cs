using System.Collections.Generic;
using _template.MasterDetail.Commands.Navigation;
using _template.MasterDetail.Resources;
using _template.MasterDetail.ViewModels.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Collections;
using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace _template.MasterDetail.ViewModels.Menu
{
    internal class MainMenuViewModel : ViewModelBase<AppCommands, AppSettings>
    {
        private Dictionary<string, ActionViewModel> _menuItems;

        public MainMenuViewModel()
        {
            Container.RegisterInstance(this);
            Title = ResourceKeys.Sample;
            Init();
        }

        public ObservableRangeCollection<ActionViewModel> MenuItems { get; } =
            new ObservableRangeCollection<ActionViewModel>();

        public string Version
        {
            get
            {
                if (Device.RuntimePlatform == Device.Android ||
                    Device.RuntimePlatform == Device.iOS ||
                    Device.RuntimePlatform == Device.UWP)
                {
                    return AppInfo.VersionString;
                }

                return typeof(App).Assembly.ImageRuntimeVersion;
            }
        }

        private void Init()
        {
            _menuItems = new Dictionary<string, ActionViewModel>
            {
                {
                    PageKind.Home.ToString(), new ActionViewModel(ResourceKeys.Main)
                    {
                        Command = Container.Resolve<NavigateToPageCommand>(),
                        Parameter = PageKind.Home
                    }
                },
                {
                    MenuKind.Localization.ToString(), new ActionViewModel(ResourceKeys.Languages)
                    {
                        Command = Container.Resolve<NavigateToMenuCommand>(),
                        Parameter = MenuKind.Localization
                    }
                },
                {
                    PageKind.Settings.ToString(), new ActionViewModel(ResourceKeys.Settings)
                    {
                        Command = Container.Resolve<NavigateToPageCommand>(),
                        Parameter = PageKind.Settings
                    }
                },
                {
                    MenuKind.About.ToString(), new ActionViewModel(ResourceKeys.About)
                    {
                        Command = Container.Resolve<NavigateToMenuCommand>(),
                        Parameter = MenuKind.About
                    }
                },
            };
            MenuItems.ReplaceAll(_menuItems.Values);
        }

        private void ActivePageKind_Changed()
        {
            foreach (var actionViewModel in MenuItems)
            {
                actionViewModel.IsActive = false;
            }

            _menuItems[ActivePageKind.ToString()].IsActive = true;
        }

        #region ActivePageKind

        /// <summary>
        ///     Gets or sets the ActivePageKind value.
        /// </summary>
        public PageKind ActivePageKind
        {
            get => _activePageKind;
            set => SetProperty(ref _activePageKind, value, ActivePageKind_Changed);
        }


        /// <summary>
        ///     ActivePageKind property data.
        /// </summary>
        private PageKind _activePageKind;

        #endregion
    }
}