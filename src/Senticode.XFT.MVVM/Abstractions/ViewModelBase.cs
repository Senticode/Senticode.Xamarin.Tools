using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core;
using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Senticode.Xamarin.Tools.Core.Helpers;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Senticode.Xamarin.Tools.MVVM.Interfaces;
using Senticode.Xamarin.Tools.MVVM.Validations;
using Senticode.Xamarin.Tools.MVVM.Validations.Interfaces;
using Unity;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     View model base for MVVM implementations.
    /// </summary>
    public abstract class ViewModelBase<TCommands, TSettings> : ViewModelBase, IAppComponentLocator<TSettings, TCommands>
        where TSettings : AppSettingsBase
        where TCommands : AppCommandsBase<TSettings>
    {
        /// <summary>
        ///     Gets the AppSettings property.
        /// </summary>
        public virtual TSettings AppSettings => Container.Resolve<TSettings>();

        /// <summary>
        ///     Gets the AppCommands property.
        /// </summary>
        public virtual TCommands AppCommands => Container.Resolve<TCommands>();
    }

    /// <summary>
    ///     View model base for MVVM implementations.
    /// </summary>
    public abstract class ViewModelBase : ModelBase, IViewModel, IDisposable, IValidity, IAppComponentLocator
    {
        private readonly List<ModelBase> _models = new List<ModelBase>();

        private readonly HashSet<string> _properties;

        private bool _isBusy;

        /// <summary>
        ///     IsInitialized property data.
        /// </summary>
        private bool _isInitialized;

        private bool _isUnsubscribed;
        private string _title;

        protected ViewModelBase()
        {
            _properties = GetType().GetProperties()
                .Select(x => x.Name)
                .ToHashSet();
            ValidateCommand = new Command(() => OnValidate());
            _title = this.GetType().Name;
        }

        /// <summary>
        ///     Gets the Container property.
        /// </summary>
        public IUnityContainer Container => ServiceLocator.Container;

        #region IDisposable

        public virtual void Dispose()
        {
            foreach (var model in _models)
            {
                model.PropertyChanged -= OnModelPropertyChanged;
            }

            _models.Clear();
        }

        #endregion


        /// <summary>
        ///     Gets or sets the IsInitialized value.
        /// </summary>
        public bool IsInitialized
        {
            get => _isInitialized;
            private set => SetProperty(ref _isInitialized, value);
        }

        /// <summary>
        ///     Gets or sets the IsBusy property.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        ///     Gets or sets the Title property.
        /// </summary>
        public virtual string Title
        {
            get => _title;
            protected internal set => SetProperty(ref _title, value);
        }

        /// <summary>
        ///     Allows you to subscribe to events OnModelPropertyChanged for all models.
        /// </summary>
        public virtual async Task OnInitializedAsync(object sender, ViewBaseEventArgs eventArgs)
        {
            IsInitialized = true;
            if (_isUnsubscribed)
            {
                await SubscribeModelAsync();
            }
        }

        /// <summary>
        ///     Allows you to unsubscribe from events OnModelPropertyChanged for all models and run other finalization actions
        /// </summary>
        public virtual async Task OnClosedAsync(object sender, ViewBaseEventArgs eventArgs)
        {
            await UnsubscribeModelAsync();
            IsInitialized = false;
        }

        /// <summary>
        ///     Handles the events occuring in models.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event arguments.</param>
        public virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_properties.Contains(e.PropertyName))
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        /// <summary>
        ///     Handles the back button pressed event.
        /// </summary>
        public virtual bool OnBackButtonPressed(Page sender)
        {
            return false;
        }

        /// <summary>
        ///     Unsubscribes from events in all models.
        /// </summary>
        /// <returns></returns>
        public virtual async Task UnsubscribeModelAsync()
        {
            await Task.Delay(0);
            foreach (var model in _models)
            {
                model.PropertyChanged -= OnModelPropertyChanged;
            }

            _isUnsubscribed = true;
        }

        /// <summary>
        ///     Subscribes to events in all models.
        /// </summary>
        /// <returns></returns>
        public virtual async Task SubscribeModelAsync()
        {
            await new TaskFactory().StartNew(() =>
            {
                foreach (var model in _models)
                {
                    model.PropertyChanged += OnModelPropertyChanged;
                }
            });
            _isUnsubscribed = false;
        }

        protected void SetModel(string property, ModelBase model)
        {
            if (model == null)
            {
                return;
            }

            GetType()
                .GetTypeInfo()
                .GetDeclaredProperty(property)
                .SetValue(this, model);
            model.PropertyChanged += OnModelPropertyChanged;
            _models.Add(model);
        }

        #region Implementation of IValidity

        #region IsValid property

        /// <summary>
        ///     Gets or sets the IsValid value.
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            private set => SetProperty(ref _isValid, value);
        }

        /// <summary>
        ///     IsValid property data.
        /// </summary>
        private bool _isValid;

        #endregion

        #region Errors property

        /// <summary>
        ///     Gets or sets the Errors value.
        /// </summary>
        public List<ErrorInfo> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        public Command ValidateCommand { get; }

        /// <summary>
        ///     Errors property data.
        /// </summary>
        private List<ErrorInfo> _errors = new List<ErrorInfo>();

        #endregion

        public Guid HandleId { get; } = Guid.NewGuid();

        public bool OnValidate()
        {
            IsValid = Errors.Count == 0;
            return IsValid;
        }

        #endregion
    }
}