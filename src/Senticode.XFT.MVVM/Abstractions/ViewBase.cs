using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.MVVM.Interfaces;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     An element that contains a single child element.
    /// </summary>
    /// <remarks>ContentView with reference to its parent.</remarks>
    public abstract class ViewBase : ContentView, IDisposable
    {
        private bool _isAppeared;
        private bool _isInitialized;

        private MasterDetailPage _masterDetailPage;

        /// <summary>
        ///     Page WeakReferenceProperty data.
        /// </summary>
        private readonly WeakReference<Page> _page = new WeakReference<Page>(default(Page));

        private IViewModel _vm;

        protected ViewBase()
        {
            MemoryInfo.Add(this);
        }

        /// <summary>
        ///     Gets or sets the WeakReferenceProperty Page value.
        /// </summary>
        private Page Page
        {
            get
            {
                _page.TryGetTarget(out var page);
                return page;
            }
            set => _page.SetTarget(value);
        }

        #region IDisposable

        public void Dispose()
        {
            Page.Appearing -= OnAppearing;
            Page.Disappearing -= OnDisappearing;
            Page = null;
        }

        #endregion

        ~ViewBase()
        {
            MemoryInfo.Remove(this);
        }

        /// <summary>Indicates that the <see cref = "T:Xamarin.Forms.Page" /> is about to appear.</summary>
        /// <remarks>To be added.</remarks>
        public event EventHandler Appearing;

        /// <summary>Indicates that the <see cref = "T:Xamarin.Forms.Page" /> is about to cease displaying.</summary>
        /// <remarks>To be added.</remarks>
        public event EventHandler Disappearing;

        protected override async void OnParentSet()
        {
            if (RealParent is Page page)
            {
                Page = page;
                if (page.RealParent is MasterDetailPage masterDetailPage)
                {
                    _masterDetailPage = masterDetailPage;
                    masterDetailPage.IsPresentedChanged += MasterDetailPageOnIsPresentedChanged;
                }
                else
                {
                    Page.Appearing += OnAppearing;
                    Page.Disappearing += OnDisappearing;
                }
            }

            if (_vm != null)
            {
                _vm.IsBusy = true;
                await InitializeViewModel();
                _vm.IsBusy = false;
            }

            base.OnParentSet();
        }

        private async void MasterDetailPageOnIsPresentedChanged(object sender, EventArgs eventArgs)
        {
            if (_masterDetailPage.IsPresented)
            {
                _vm.IsBusy = true;
                await _vm.OnInitializedAsync(this, new ViewBaseEventArgs(Page));
                _vm.IsBusy = false;
            }
            else
            {
                _vm.IsBusy = true;

                try
                {
                    await _vm.OnClosedAsync(this, new ViewBaseEventArgs(Page));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                _masterDetailPage.IsPresentedChanged += MasterDetailPageOnIsPresentedChanged;
                _vm.IsBusy = false;
            }
        }

        private async Task InitializeViewModel()
        {
            if (!_isInitialized && _isAppeared && _vm != null)
            {
                _isInitialized = true;
                _vm.IsBusy = true;
                await _vm.OnInitializedAsync(this, new ViewBaseEventArgs(Page));
                _vm.IsBusy = false;
            }
        }

        protected override async void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is IViewModel vm)
            {
                _vm = vm;
                _vm.IsBusy = true;
                await InitializeViewModel();
                _vm.IsBusy = false;
            }
            else if (_vm != null)
            {
                _vm.IsBusy = true;
                await _vm.OnClosedAsync(this, new ViewBaseEventArgs(Page));
                _vm.IsBusy = false;
                _vm = null;
                _isInitialized = false;
            }
        }

        private async void OnDisappearing(object sender, EventArgs eventArgs)
        {
            Disappearing?.Invoke(this, eventArgs);
            if (_vm != null)
            {
                _vm.IsBusy = true;
                await _vm.OnClosedAsync(this, new ViewBaseEventArgs(Page));
                _vm.IsBusy = false;
            }

            _isAppeared = false;
            _isInitialized = false;
        }

        private async void OnAppearing(object sender, EventArgs eventArgs)
        {
            Appearing?.Invoke(this, eventArgs);
            _isAppeared = true;
            if (_vm != null)
            {
                _vm.IsBusy = true;
                await _vm.OnInitializedAsync(this, new ViewBaseEventArgs(Page));
                _vm.IsBusy = false;
            }
        }
    }
}