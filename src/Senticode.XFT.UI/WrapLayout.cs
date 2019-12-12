using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.UI
{
    public class WrapLayout : Layout<View>
    {
        /// <summary>
        /// Backing Storage for the Orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation),
                typeof(StackOrientation), typeof(WrapLayout), StackOrientation.Vertical, BindingMode.OneWay, null,
                (bindable, value, newValue) => ((WrapLayout)bindable).OnSizeChanged());

        /// <summary>
        /// Orientation (Horizontal or Vertical).
        /// </summary>
        public StackOrientation Orientation
        {
            get => (StackOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// Backing Storage for the Spacing property.
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing),
            typeof(double), typeof(WrapLayout), 6.0, BindingMode.OneWay, null,
            (bindable, value, newValue) => ((WrapLayout)bindable).OnSizeChanged());

        /// <summary>
        /// Spacing added between elements (both directions).
        /// </summary>
        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        /// <summary>
        /// Backing Storage for the Spacing property.
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate),
            typeof(DataTemplate), typeof(WrapLayout), null, BindingMode.OneWay, null,
            (bindable, value, newValue) => ((WrapLayout)bindable).OnSizeChanged());

        /// <summary>
        /// Spacing added between elements (both directions).
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Backing Storage for the Spacing property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(WrapLayout),
                null,
                BindingMode.OneWay,
                null,
                propertyChanged: ItemsSource_OnPropertyChanged);

        private double _width;
        private double _height;

        /// <summary>
        /// Spacing added between elements (both directions).
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        


        private static void ItemsSource_OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            IEnumerable newValueAsEnumerable;
            try
            {
                newValueAsEnumerable = newValue as IEnumerable;
            }
            catch (Exception e)
            {
                throw e;
            }

            var control = (WrapLayout)bindable;

            if (oldValue is INotifyCollectionChanged oldObservableCollection)
            {
                oldObservableCollection.CollectionChanged -= control.ItemsSource_Changed;
            }

            if (newValue is INotifyCollectionChanged newObservableCollection)
            {
                newObservableCollection.CollectionChanged += control.ItemsSource_Changed;
            }

            control.Children.Clear();

            if (newValueAsEnumerable != null)
            {
                foreach (var item in newValueAsEnumerable)
                {
                    var view = control.CreateChildViewFor(item);
                    control.Children.Add(view);
                }
            }
        }

        private void ItemsSource_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            object item;
            View view;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        for (var i = 0; i < e.NewItems.Count; ++i)
                        {
                            item = e.NewItems[i];
                            view = CreateChildViewFor(item);

                            Children.Insert(i + e.NewStartingIndex, view);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Children.RemoveAt(e.OldStartingIndex);
                    item = e.NewItems[e.NewStartingIndex];
                    view = CreateChildViewFor(item);
                    Children.Insert(e.NewStartingIndex, view);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        Children.RemoveAt(e.OldStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    break;
            }
        }

        private View CreateChildViewFor(object item)
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

        /// <summary>
        /// This is called when the spacing or orientation properties are changed - it forces
        /// the control to go back through a layout pass.
        /// </summary>
        private void OnSizeChanged()
        {
            ForceLayout();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Equals(_width, width) && Equals(_height, height))
            {
                return;
            }

            _width = width;
            _height = height;

            ForceLayout();
        }

        /// <summary>
        /// Get the desired size of an element.
        /// </summary>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (WidthRequest > 0)
            {
                widthConstraint = Math.Min(widthConstraint, WidthRequest);
            }

            if (HeightRequest > 0)
            {
                heightConstraint = Math.Min(heightConstraint, HeightRequest);
            }

            var internalWidth = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0, widthConstraint);
            var internalHeight = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0, heightConstraint);

            if (Orientation == StackOrientation.Vertical)
            {
                return GetVerticalSize(internalWidth, internalHeight);
            }
            return GetHorizontalSize(internalWidth, internalHeight);
        }

        /// <summary>
        /// Get the vertical measure.
        /// </summary>
        private SizeRequest GetVerticalSize(double widthConstraint, double heightConstraint)
        {
            var columnCount = 1;

            double width = 0;
            double height = 0;
            double minWidth = 0;
            double minHeight = 0;
            double heightUsed = 0;

            foreach (var item in Children)
            {
                var size = item.Measure(widthConstraint, heightConstraint);
                width = Math.Max(width, size.Request.Width);

                var newHeight = height + size.Request.Height + Spacing;
                if (newHeight > heightConstraint)
                {
                    columnCount++;
                    heightUsed = Math.Max(height, heightUsed);
                    height = size.Request.Height;
                }
                else
                {
                    height = newHeight;
                }

                minHeight = Math.Max(minHeight, size.Minimum.Height);
                minWidth = Math.Max(minWidth, size.Minimum.Width);
            }

            if (columnCount > 1)
            {
                height = Math.Max(height, heightUsed);
                width *= columnCount;  
            }

            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        /// <summary>
        /// Get the horizontal measure.
        /// </summary>
        private SizeRequest GetHorizontalSize(double widthConstraint, double heightConstraint)
        {
            var rowCount = 1;

            double width = 0;
            double height = 0;
            double minWidth = 0;
            double minHeight = 0;
            double widthUsed = 0;

            foreach (var item in Children)
            {
                var size = item.Measure(widthConstraint, heightConstraint);
                height = Math.Max(height, size.Request.Height);

                var newWidth = width + size.Request.Width + Spacing;
                if (newWidth > widthConstraint)
                {
                    rowCount++;
                    widthUsed = Math.Max(width, widthUsed);
                    width = size.Request.Width;
                }
                else
                {
                    width = newWidth;
                }

                minHeight = Math.Max(minHeight, size.Minimum.Height);
                minWidth = Math.Max(minWidth, size.Minimum.Width);
            }

            if (rowCount > 1)
            {
                width = Math.Max(width, widthUsed);
                height = (height + Spacing) * rowCount - Spacing; 
            }

            return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
        }

        /// <summary>
        /// Positions and sizes the children of a Layout.
        /// </summary>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (Orientation == StackOrientation.Vertical)
            {
                double colWidth = 0;
                double yPos = y, xPos = x;

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    var request = child.Measure(width, height);

                    var childWidth = request.Request.Width;
                    var childHeight = request.Request.Height;
                    colWidth = Math.Max(colWidth, childWidth);

                    if (yPos + childHeight > height)
                    {
                        yPos = y;
                        xPos += colWidth + Spacing;
                        colWidth = 0;
                    }

                    var region = new Rectangle(xPos, yPos, childWidth, childHeight);
                    
                    LayoutChildIntoBoundingRegion(child, region);
                    yPos += region.Height + Spacing;
                }
            }
            else
            {
                double rowHeight = 0;
                double yPos = y, xPos = x;

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    var request = child.Measure(width, double.PositiveInfinity);
                    var childHeight = request.Request.Height;
                    rowHeight = Math.Max(rowHeight, childHeight);
                }

                foreach (var child in Children.Where(c => c.IsVisible))
                {
                    var request = child.Measure(width, double.PositiveInfinity);

                    var childWidth = request.Request.Width;
                    var childHeight = request.Request.Height;

                    if (xPos + childWidth > width)
                    {
                        xPos = x;
                        yPos += rowHeight + Spacing;
                    }

                    var region = new Rectangle(xPos, yPos + rowHeight - childHeight, childWidth, childHeight);
                    LayoutChildIntoBoundingRegion(child, region);
                    xPos += region.Width + Spacing;
                }

            }
        }
    }
}
