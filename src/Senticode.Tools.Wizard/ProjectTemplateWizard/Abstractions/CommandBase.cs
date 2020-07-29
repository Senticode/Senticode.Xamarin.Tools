using System;
using System.Windows.Input;

namespace ProjectTemplateWizard.Abstractions
{
    internal abstract class CommandBase : ICommand
    {
        protected bool IsCanExecute { get; private set; } = true;

        public virtual bool CanExecute(object parameter) => IsCanExecute;

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;

        protected void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Disable()
        {
            IsCanExecute = false;
            ChangeCanExecute();
        }

        public void Enable()
        {
            IsCanExecute = true;
            ChangeCanExecute();
        }
    }
}