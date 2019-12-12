using System;
using System.Collections.Generic;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Senticode.Xamarin.Tools.MVVM.Validations.Interfaces;

namespace Senticode.Xamarin.Tools.MVVM.Validations
{
    public partial class ValidatableObject<T>
    {
        private WeakReference<ModelBase> _model;
        private string _propertyName;

        /// <summary>
        ///     Converts model property to ValidatableObject.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>ValidatableObject.</returns>
        public ValidatableObject<T> FromModel<TModel>(TModel model, string propertyName)
            where TModel : ModelBase
        {
            _propertyName = propertyName;
            _model = new WeakReference<ModelBase>(model);
            return this;
        }

        private void OnSetValueToModel(T value)
        {
            if (_model.TryGetTarget(out var model))
            {
                model.TrySetValue(value, _propertyName);
            }
        }

        /// <summary>
        ///     Adds new validation rule.
        /// </summary>
        /// <param name="rule">Validation rule.</param>
        /// <returns>ValidatableObject.</returns>
        public ValidatableObject<T> AddRule(IValidationRule<T> rule)
        {
            Validations.Add(rule);
            return this;
        }

        /// <summary>
        ///     Adds a range of validation rules.
        /// </summary>
        /// <param name="rule">Validation rules.</param>
        /// <returns>ValidatableObject.</returns>
        public ValidatableObject<T> AddRules(IEnumerable<IValidationRule<T>> rule)
        {
            Validations.AddRange(rule);
            return this;
        }
    }
}