using System;
using System.Windows.Input;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Staff
{
    /// <summary>
    ///     Class that represents a command.
    /// </summary>
    public abstract class CommandBase: ICommand
    {
        #region Implementation of ICommand

        /// <summary>
        ///     Determines whether the command can execute in its current state.
        /// </summary>        
        public virtual bool CanExecute(object parameter)
        {
            return IsCanExecute;
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        public abstract void Execute(object parameter);

        /// <summary>
        ///    Occurs when changes occur that affect whether or not the command should execute. 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        protected void ChangeCanExecute()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        protected bool IsCanExecute { get; private set; } = true;

        /// <summary>
        ///     Prevents command execution.
        /// </summary>
        public void Disable()
        {
            IsCanExecute = false;
            ChangeCanExecute();
        }

        /// <summary>
        ///     Enables the possibility to execute command.
        /// </summary>
        public void Enable()
        {
            IsCanExecute = true;
            ChangeCanExecute();
        }

        #endregion
    }
}
