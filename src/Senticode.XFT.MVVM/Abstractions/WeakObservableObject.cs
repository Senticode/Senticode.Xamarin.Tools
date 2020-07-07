using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Senticode.Xamarin.Tools.Core.Events.Managers;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     ObservableObject with weak events.
    /// </summary>
    public abstract class WeakObservableObject : ObservableObject
    {
        #region Overrides INotifyPropertyChanged

        private List<WeakReference<PropertyChangedEventHandler>> _changedEventHandlers =
            new List<WeakReference<PropertyChangedEventHandler>>();

        /// <summary>
        ///     Occurs when property changes.
        /// </summary>
        public new event PropertyChangedEventHandler PropertyChanged
        {
            add => PropertyChangedWeakEventManager.AddHandler(ref _changedEventHandlers, value);
            remove => PropertyChangedWeakEventManager.RemoveHandler(_changedEventHandlers, value);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedWeakEventManager.CallHandlers(this, new PropertyChangedEventArgs(propertyName),
                _changedEventHandlers);
        }

        #endregion
        
        #region Overrides INotifyPropertyChanging

        private List<WeakReference<PropertyChangingEventHandler>> _changingEventHandlers =
            new List<WeakReference<PropertyChangingEventHandler>>();

        /// <summary>
        ///     Occurs when property is changing.
        /// </summary>
        public new event PropertyChangingEventHandler PropertyChanging
        {
            add => PropertyChangingWeakEventManager.AddHandler(ref _changingEventHandlers, value);
            remove => PropertyChangingWeakEventManager.RemoveHandler(_changingEventHandlers, value);
        }

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = "")
        {
            PropertyChangingWeakEventManager.CallHandlers(this, new PropertyChangingEventArgs(propertyName),
                _changingEventHandlers);
        }

        #endregion
    }
}