using System;
using System.Collections.Generic;
using System.Linq;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Validations.Interfaces;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Validations
{
    /// <summary>
    ///     Contains data that needs to be validated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ValidatableObject<T> : ObservableObject, IValidity
    {
        private readonly WeakReference<IValidity> _viewmodel;

        public ValidatableObject(IValidity viewmodel) : this()
        {
            _viewmodel = new WeakReference<IValidity>(viewmodel);
        }

        public ValidatableObject()
        {
            HandleId = Guid.NewGuid();
            ValidateCommand = new Command(() => OnValidate());
        }

        /// <summary>
        ///     List of validations.
        /// </summary>
        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        /// <summary>
        ///     Gets the HandleId property.
        /// </summary>
        public Guid HandleId { get; }

        /// <summary>
        ///     Method used for validation.
        /// </summary>
        /// <returns><c>true</c> if object is valid, otherwise, <c>false</c>.</returns>
        public bool OnValidate()
        {
            Errors.Clear();
            Errors = Validations
                .Where(v => !v.Check(Value))
                .Select(v => new ErrorInfo(HandleId, v.ValidationMessage))
                .ToList();

            IsValid = Errors.Count == 0;

            ValidateViewModel();

            return IsValid;
        }

        private void ValidateViewModel()
        {
            if (_viewmodel.TryGetTarget(out var vm))
            {
                var removeElements = new List<ErrorInfo>();
                foreach (var err in Errors)
                {
                    if (err.HandleId == HandleId)
                    {
                        removeElements.Add(err);
                    }
                }

                foreach (var err in removeElements)
                {
                    vm?.Errors.Remove(err);
                }


                vm?.Errors.AddRange(Errors);
                vm.OnValidate();
            }
        }

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
        private bool _isValid = true;

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

        #region Value property

        /// <summary>
        ///     Gets or sets the Value.
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                OnValidate();
                if (_propertyName != null)
                {
                    OnSetValueToModel(value);
                }
            }
        }

        /// <summary>
        ///     Value property data.
        /// </summary>
        private T _value;

        #endregion
    }
}