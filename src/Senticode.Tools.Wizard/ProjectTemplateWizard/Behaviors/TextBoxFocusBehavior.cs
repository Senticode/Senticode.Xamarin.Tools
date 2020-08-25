using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ProjectTemplateWizard.Behaviors
{
    internal class TextBoxFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObjectOnLayoutUpdated;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObjectOnLayoutUpdated;
        }

        private void AssociatedObjectOnLayoutUpdated(object sender, EventArgs e)
        {
            AssociatedObject.CaretIndex = AssociatedObject.Text.Length;
            AssociatedObject.Focus();
        }
    }
}