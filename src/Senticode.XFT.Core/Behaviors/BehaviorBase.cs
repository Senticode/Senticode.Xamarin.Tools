using System;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Behaviors
{
    /// <summary>
    ///     Base generic class for generalized user-defined behaviors that can respond to arbitrary conditions and events.
    /// </summary>
    /// <typeparam name="T">The type of the objects with which this Behavior<T> can be associated.</typeparam>
    public abstract class BehaviorBase<T> : Behavior<T> where T : BindableObject
    {
        protected T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}
