using System;
using System.Windows.Input;

namespace MediaPortal.MVVM.Core
{
    /// <summary>
    /// A command that implements the ICommand interface for use in MVVM patterns.
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Action<object> _execute; // Action to execute when the command is invoked
        private readonly Func<object, bool> _canExecute; // Function to determine if the command can execute

        /// Event that fires when changes occur that affect whether the command should execute.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; } // Subscribe to CommandManager's RequerySuggested event
            remove { CommandManager.RequerySuggested -= value; } // Unsubscribe from CommandManager's RequerySuggested event
        }

        /// Initializes a new instance of the RelayCommand class.
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">Optional function to determine if the command can execute.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// Determines whether the command can execute in its current state.
        /// <param name="parameter">Data used by the command.</param>
        /// <returns>True if the command can execute, otherwise false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// Executes the command action.
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
