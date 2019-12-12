using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.UI
{

    public class ItemsStackLayout : StackLayout
    {
        /// <summary>
        /// Gets or sets the ItemsSource value.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// ItemsSource property data.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ItemsStackLayout), default(IEnumerable<object>), BindingMode.TwoWay, null, ItemsSourceChanged);

        private static void ItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var itemsLayout = (ItemsStackLayout)bindable;
            itemsLayout.SetItems();
        }


        /// <summary>
        /// Gets or sets the ItemTemplate value.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// ItemTemplate property data.
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ItemsStackLayout), default(DataTemplate));


        protected virtual void SetItems()
        {
            Children.Clear();

            if (ItemsSource == null)
            {
                ObservableSource = null;
                return;
            }

            foreach (var item in ItemsSource)
            {
                Children.Add(GetItemView(item));
            }

            var isObs = ItemsSource.GetType().IsConstructedGenericType && ItemsSource.GetType().GetGenericTypeDefinition() == typeof(ObservableCollection<>);
            if (isObs)
            {
                ObservableSource = new ObservableCollection<object>(ItemsSource.Cast<object>());
            }
        }

        protected virtual View GetItemView(object item)
        {
            object content;
            if (ItemTemplate is DataTemplateSelector itemTemplateSelector)
            {
                content = itemTemplateSelector.SelectTemplate(item, this).CreateContent();
            }
            else
            {
                content = ItemTemplate?.CreateContent();
            }

            View view;
            if (content is ViewCell vc)
            {
                view = vc.View;
            }
            else
            {
                view = (View)content;
            }

            try
            {
                var bindableView = (BindableObject)view;
                if (bindableView != null && item != null)
                {
                    bindableView.BindingContext = item;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return view;
        }



        ObservableCollection<object> _observableSource;
        protected ObservableCollection<object> ObservableSource
        {
            get => _observableSource;
            set
            {
                if (_observableSource != null)
                {
                    _observableSource.CollectionChanged -= CollectionChanged;
                }
                _observableSource = value;

                if (_observableSource != null)
                {
                    _observableSource.CollectionChanged += CollectionChanged;
                }
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        int index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                            Children.Insert(index++, GetItemView(item));
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    {
                        var item = ObservableSource[e.OldStartingIndex];
                        Children.RemoveAt(e.OldStartingIndex);
                        Children.Insert(e.NewStartingIndex, GetItemView(item));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        Children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    {
                        Children.RemoveAt(e.OldStartingIndex);
                        Children.Insert(e.NewStartingIndex, GetItemView(ObservableSource[e.NewStartingIndex]));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    foreach (var item in ItemsSource)
                        Children.Add(GetItemView(item));
                    break;
            }
        }

    }
}
