using System;
using ProjectTemplateWizard.Abstractions;

namespace ProjectTemplateWizard.Core
{
    internal class Command : CommandBase
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public Command(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (!IsCanExecute)
            {
                return false;
            }

            if (_canExecute != null)
            {
                return _canExecute();
            }

            return true;
        }

        public override void Execute(object parameter)
        {
            _execute?.Invoke();
        }
    }

    internal class Command<T> : CommandBase
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public Command(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (!IsCanExecute)
            {
                return false;
            }

            if (_canExecute != null)
            {
                return _canExecute((T) parameter);
            }

            return true;
        }

        public override void Execute(object parameter)
        {
            _execute?.Invoke((T) parameter);
        }
    }
}