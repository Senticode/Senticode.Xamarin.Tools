using System;
using System.Collections.Generic;
using System.Diagnostics;
using _template.MasterDetail.ViewModels.Menu;
using _template.MasterDetail.Views;
using _template.MasterDetail.Views.Menu;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Unity;
using Xamarin.Forms;

namespace _template.MasterDetail.Commands.Navigation
{
    public enum MenuKind
    {
        Main,
        About,
        Localization,
        Licenses
    }

    public class NavigateToMenuCommand : CommandBase
    {
        private readonly Dictionary<MenuKind, Func<ViewViewModelPair>> _menuViews;
        private readonly Dictionary<MenuKind, Action> _postNavigateActions;

        public NavigateToMenuCommand(IUnityContainer container)
        {
            _menuViews = new Dictionary<MenuKind, Func<ViewViewModelPair>>
            {
                {
                    MenuKind.Main,
                    () => new ViewViewModelPair(container.Resolve<MainMenu>(), container.Resolve<MainMenuViewModel>())
                },
                {
                    MenuKind.About,
                    () => new ViewViewModelPair(container.Resolve<AboutMenu>(),
                        container.Resolve<AboutMenuViewModel>())
                },
                {
                    MenuKind.Localization,
                    () => new ViewViewModelPair(container.Resolve<LanguageMenu>(),
                        container.Resolve<LanguageMenuViewModel>())
                }
            };

            _postNavigateActions = new Dictionary<MenuKind, Action>
            {
                {MenuKind.Main, () => { }},
            };


            container.RegisterInstance(this);
        }

        public override void Execute(object parameter)
        {
            Disable();

            try
            {
                if (parameter is MenuKind menuType)
                {
                    var mainPage = (MainPage) Application.Current.MainPage;
                    var master = (ContentPage) mainPage.Master;

                    if (_menuViews.TryGetValue(menuType, out var action))
                    {
                        var vvm = action.Invoke();
                        master.Content = vvm.View;
                        master.Content.BindingContext = vvm.ViewModel;
                    }

                    if (_postNavigateActions.TryGetValue(menuType, out var postAction))
                    {
                        postAction?.Invoke();
                    }
                }
                else
                {
                    Debug.Fail($"Wrong type of parameter for {nameof(NavigateToMenuCommand)}");
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