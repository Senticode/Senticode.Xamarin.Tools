using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Senticode.Xamarin.Tools.MVVM.Collections;
using Senticode.Xamarin.Tools.MVVM.Interfaces;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM
{
    /// <summary>
    ///     Represents a collection of objects that can be grouped based on specified key.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public class Grouping<TKey, TModel> : ObservableRangeCollection<TModel>, IGrouping<TKey, TModel>, IGroupable
        where TModel : class, IGroupable
    {
        private Command _hideCommand;


        /// <summary>
        ///     IsActive property data.
        /// </summary>
        private bool _isActive;

        private bool _isHidden;
        
        public Grouping()
        {
            IsActive = true;
        }

        
        public Grouping(TKey key)
        {
            Key = key;
        }

        public Grouping(TKey key, IEnumerable<TModel> items)
            : this(key)
        {
            AddRange(items);
        }

        public Grouping(TKey key, IEnumerable<TModel> items, int columnCount)
            : this(key, items)
        {
            ColumnCount = columnCount;
        }

        /// <summary>
        ///     Gets the ColumnCount property.
        /// </summary>
        public int ColumnCount { get; }

        /// <summary>
        ///     Gets the Hide command.
        /// </summary>
        public Command HideCommand => _hideCommand ??
                                      (_hideCommand = new Command(ExecuteHide));

        /// <summary>
        ///     Gets or sets the IsHidden property.
        /// </summary>
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value;
                HiddenChildren(value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsHidden)));
            }
        }

        /// <summary>
        ///     Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    ActivateChildren(value);
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        /// <summary>
        ///     Gets the key for grouping.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        ///     Method to invoke when the Hide command is executed.
        /// </summary>
        private void ExecuteHide()
        {
            IsHidden = !IsHidden;
        }


        private void HiddenChildren(bool value)
        {
            foreach (var model in Items)
            {
                if (model.IsHidden != value)
                {
                    model.IsHidden = value;
                }
            }
        }

        private void ActivateChildren(bool value)
        {
            foreach (var model in Items)
            {
                if (model.IsActive != value)
                {
                    model.IsActive = value;
                }
            }
        }
    }
}