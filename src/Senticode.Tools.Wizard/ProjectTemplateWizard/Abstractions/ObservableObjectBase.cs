using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ProjectTemplateWizard.Abstractions.Interfaces;

namespace ProjectTemplateWizard.Abstractions
{
    internal abstract class ObservableObjectBase : IObservableObject
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public virtual event PropertyChangingEventHandler PropertyChanging;

        public bool SetProperty<T>(
            ref T backingStore, T value,
            Action onChanged = null,
            Action onChanging = null,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            OnPropertyChanging(propertyName, onChanging);
            backingStore = value;
            OnPropertyChanged(propertyName, onChanged);

            return true;
        }

        private void OnPropertyChanging(string propertyName, Action onChanging)
        {
            onChanging?.Invoke();
            OnPropertyChanging(propertyName);
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private void OnPropertyChanged(string propertyName, Action onChanged)
        {
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}