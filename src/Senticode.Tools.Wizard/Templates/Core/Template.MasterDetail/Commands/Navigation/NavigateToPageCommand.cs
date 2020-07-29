using System;
using System.Collections.Generic;
using System.Diagnostics;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Template.MasterDetail.ViewModels;
using Template.MasterDetail.ViewModels.Menu;
using Template.MasterDetail.Views.Pages;
using Unity;
using Xamarin.Forms;

namespace Template.MasterDetail.Commands.Navigation
{
    public enum PageKind
    {
        Home,
        Settings
    }

    public class NavigateToPageCommand : CommandBase
    {
        private readonly Dictionary<PageKind, Func<PageViewModelPair>> _menuViews;
        private readonly Dictionary<PageKind, Action> _postNavigateActions;

        public NavigateToPageCommand(IUnityContainer container)
        {
            var container1 = container;

            _menuViews = new Dictionary<PageKind, Func<PageViewModelPair>>
            {
                {
                    PageKind.Home,
                    () => new PageViewModelPair(container1.Resolve<HomePage>(),
                        container1.Resolve<HomeViewModel>())
                },
                {
                    PageKind.Settings,
                    () => new PageViewModelPair(container1.Resolve<SettingsPage>(),
                        container1.Resolve<SettingsViewModel>())
                },
            };

            _postNavigateActions = new Dictionary<PageKind, Action>
            {
                {
                    PageKind.Home,
                    () => { container1.Resolve<MainMenuViewModel>().ActivePageKind = PageKind.Home; }
                }
            };
            container.RegisterInstance(this);
        }

        public PageKind LastPageKind { get; private set; } = PageKind.Home;

        public override void Execute(object parameter)
        {
            Disable();

            try
            {
                if (parameter is PageKind pageKind)
                {
                    LastPageKind = pageKind;

                    if (_menuViews.TryGetValue(pageKind, out var action))
                    {
                        var vvm = action.Invoke();
                        vvm.Page.BindingContext = vvm.ViewModel;

                        if (Application.Current.MainPage is MasterDetailPage masterDetailPageOwner)
                        {
                            var navigationPage = masterDetailPageOwner?.Detail as NavigationPage;

                            if (navigationPage?.CurrentPage?.GetType() != vvm.Page.GetType())
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    masterDetailPageOwner.Detail = new NavigationPage(vvm.Page);
                                    masterDetailPageOwner.IsPresented = false;
                                });
                            }
                            else
                            {
                                masterDetailPageOwner.IsPresented = false;
                            }
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() => { Application.Current.MainPage = vvm.Page; });
                        }
                    }

                    if (_postNavigateActions.TryGetValue(pageKind, out var postAction))
                    {
                        postAction?.Invoke();
                    }
                }
                else
                {
                    Debug.Fail($"Wrong type of parameter for {nameof(NavigateToPageCommand)}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                Enable();
            }
        }
    }
}