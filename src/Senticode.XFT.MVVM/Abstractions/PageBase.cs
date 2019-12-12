using System.ComponentModel;
using Senticode.Xamarin.Tools.MVVM.Interfaces;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     A <c>ContentPage</c> that displays a single view.
    /// </summary>
    public abstract class PageBase : ContentPage
    {
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is IViewModel viewModel)
            {
                ViewModel = viewModel;
                await viewModel.OnInitializedAsync(this, new ViewBaseEventArgs(this));
                ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is IViewModel viewModel)
            {
                await viewModel.OnClosedAsync(this, new ViewBaseEventArgs(this));
                viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
        }

        /// <summary>
        ///     Gets or sets the ViewModel property.
        /// </summary>
        public IViewModel ViewModel { get; set; }

        #region Overrides of ContentPage

        protected override async void OnBindingContextChanged()
        {
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
            if (BindingContext is IViewModel viewModel)
            {
                ViewModel = viewModel;
                await viewModel.OnInitializedAsync(this, new ViewBaseEventArgs(this));
                ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
            base.OnBindingContextChanged();
        }

        #endregion

        protected override bool OnBackButtonPressed()
        {
            if (BindingContext is IViewModel viewModel)
            {
                return viewModel.OnBackButtonPressed(this);
            }
            return base.OnBackButtonPressed();
        }

        protected virtual void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
          
        }
    }
}