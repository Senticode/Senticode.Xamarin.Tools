using System;
using System.Collections.Generic;
using System.Reflection;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Attributes;
using Unity;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM
{
    /// <summary>
    ///     Creates dialog pages.
    /// </summary>
    public class DialogFactory<TModalPage, TPopupPage> where TModalPage : ContentPage where TPopupPage : ContentPage
    {
        private readonly IUnityContainer _container;

        private readonly Dictionary<Type, TModalPage> _contentPages = new Dictionary<Type, TModalPage>();

        private readonly Dictionary<Type, TPopupPage> _popupPages = new Dictionary<Type, TPopupPage>();

        public DialogFactory(IUnityContainer container)
        {
            _container = container;
            container.RegisterInstance(this);
        }

        /// <summary>
        ///     Creates a popup page.
        /// </summary>
        /// <typeparam name="T">ViewModel type.</typeparam>
        public TPopupPage ComposeDialog<T>() where T : ViewModelBase
        {
            var view = ComposeView<T>();
            return ComposeDialogFromContentView(view);
        }

        /// <summary>
        ///     Creates a modal page.
        /// </summary>
        /// <typeparam name="T">ViewModel type.</typeparam>
        public TModalPage ComposePage<T>() where T : ViewModelBase
        {
            var view = ComposeView<T>();
            return ComposePageFromContentView(view);
        }

        /// <summary>
        ///     Creates a view for dialog.
        /// </summary>
        /// <typeparam name="T">ViewModel type.</typeparam>
        /// <returns>Returns view.</returns>
        public ViewBase ComposeView<T>() where T : ViewModelBase
        {
            var viewModel = GetDialogViewModel<T>();

            var requiredViewAttribute = viewModel.GetType().GetTypeInfo()
                .GetCustomAttribute<RequiredViewAttribute>();

            if (requiredViewAttribute?.ContractType == null)
            {
                throw new ArgumentException($"{typeof(T)} not supported view type");
            }

            var dialogView = (ViewBase) _container.Resolve(requiredViewAttribute.ContractType);
            dialogView.BindingContext = viewModel;

            return dialogView;
        }

        private TPopupPage ComposeDialogFromContentView(ViewBase view)
        {
            TPopupPage page;
            var viewType = view.GetType();
            if (_popupPages.ContainsKey(viewType))
            {
                page = _popupPages[viewType];
                //TODO: EM if you will use container-instance ViewBase, you have to delete the next code-line (setter page.Content).
                page.Content = view;
            }
            else
            {
                page = Activator.CreateInstance<TPopupPage>();
                page.Content = view;
                _popupPages.Add(viewType, page);
            }

            page.BindingContext = view.BindingContext;
            return page;
        }

        private TModalPage ComposePageFromContentView(ViewBase view)
        {
            TModalPage page;
            var viewType = view.GetType();
            if (_contentPages.ContainsKey(viewType))
            {
                page = _contentPages[viewType];
                //TODO: EM if you will use container-instance ViewBase, you have to delete the next code-line (setter page.Content).
                page.Content = view;
            }
            else
            {
                page = Activator.CreateInstance<TModalPage>();
                page.Content = view;
                _contentPages.Add(viewType, page);
            }

            page.BindingContext = view.BindingContext;
            return page;
        }

        private T GetDialogViewModel<T>() where T : ViewModelBase
        {
            return _container.Resolve<T>();
        }
    }
}