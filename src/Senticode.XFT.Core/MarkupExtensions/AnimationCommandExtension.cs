using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Senticode.Xamarin.Tools.Core.Helpers;
using Senticode.Xamarin.Tools.Core.Interfaces.Staff;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.MarkupExtensions
{
    [ContentProperty(nameof(Binding))]
    public class AnimationCommandExtension : WeakMarkupExtensionBase, IMarkupExtension
    {
        private BindableObject _bo;
        private object _parameter;
        private BindableProperty _property;

        public IAnimation StartAnimation { get; set; }
        public IAnimation EndAnimation { get; set; }

        public BindingBase Binding { get; set; }


        public object ProvideValue(IServiceProvider serviceProvider)
        {
            SetProvideValue(serviceProvider);
            SetBindableObject(serviceProvider);
            if (Binding != null)
            {
                if (!(Binding is Binding binding) || string.IsNullOrWhiteSpace(binding.Path))
                {
                    throw new InvalidOperationException($"'{nameof(Binding)}' must be properly set.");
                }

                SetBindableObject(serviceProvider);
                if (_bo != null)
                {
                    _bo.BindingContextChanged += AnimationCommandExtension_BindingContextChanged;
                }

                if (_bo != null)
                {
                    _bo.PropertyChanged += BoOnPropertyChanged;
                }

                return null;
            }

            if (_bo != null)
            {
                _bo.BindingContextChanged -= AnimationCommandExtension_BindingContextChanged;
            }

            if (_bo != null)
            {
                _bo.PropertyChanged -= BoOnPropertyChanged;
            }

            return null;
        }

        public override void Subscribe()
        {
            if (_bo != null)
            {
                _bo.BindingContextChanged += AnimationCommandExtension_BindingContextChanged;
                _bo.PropertyChanged += BoOnPropertyChanged;
            }
        }

        public override void Unsubscribe()
        {
            if (_bo != null)
            {
                _bo.BindingContextChanged -= AnimationCommandExtension_BindingContextChanged;
                _bo.PropertyChanged -= BoOnPropertyChanged;
            }
        }

        private void BoOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CommandParameter")
            {
                var bo = (BindableObject) sender;
                _parameter = null;
                try
                {
                    var info = bo.GetType().GetRuntimeProperty("CommandParameter");
                    if (info != null)
                    {
                        _parameter = info.GetValue(bo);
                    }
                }
                catch
                {
                    _parameter = null;
                }
            }
        }

        private void AnimationCommandExtension_BindingContextChanged(object sender, EventArgs e)
        {
            var bo = (BindableObject) sender;
            try
            {
                //Command wrap
                var command = new Command(async () =>
                {
                    var control = (View) ((GestureRecognizer) sender).Parent;
                    var animationToPlay = StartAnimation ?? new DefaultAnimation();
                    try
                    {
                        if (control != null)
                        {
                            await animationToPlay.Play(control);
                        }
                    }
                    catch
                    {
                        Debug.WriteLine($"{nameof(AnimationCommandExtension)}.StartAnimation Exception");
                    }

                    try
                    {
                        var baseCommand = (ICommand) MarkupExtensionHelper.ExtractMember(bo, (Binding) Binding);
                        if (baseCommand != null && baseCommand.CanExecute(_parameter))
                        {
                            baseCommand.Execute(_parameter);
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }

                    try
                    {
                        if (EndAnimation != null && control != null)
                        {
                            await EndAnimation.Play(control);
                        }
                    }
                    catch
                    {
                        Debug.WriteLine($"{nameof(AnimationCommandExtension)}.EndAnimation Exception");
                    }
                });

                bo.SetValue(_property, command);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                bo.SetValue(_property, null);
            }
        }

        private void SetBindableObject(IServiceProvider serviceProvider)
        {
            var pvt = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            _bo = pvt.TargetObject as BindableObject;
            _property = pvt.TargetProperty as BindableProperty;
        }

        private class DefaultAnimation : IAnimation
        {
            public async Task Play(View sender)
            {
                try
                {
                    await sender.ScaleTo(0.8, 50);
                    await sender.ScaleTo(1, 50);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}