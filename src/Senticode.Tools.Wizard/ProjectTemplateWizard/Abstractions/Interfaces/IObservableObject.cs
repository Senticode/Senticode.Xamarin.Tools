using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectTemplateWizard.Abstractions.Interfaces
{
    internal interface IObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        bool SetProperty<T>(
            ref T backingStore, T value,
            Action onChanged = null,
            Action onChanging = null,
            [CallerMemberName] string propertyName = "");
    }
}