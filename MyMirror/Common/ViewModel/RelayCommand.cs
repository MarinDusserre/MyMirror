// -----------------------------------------------------------------------
// <copyright file="RelayCommand.cs">
// Taken from <see cref="http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030"/>
// </copyright>
// <summary>Contains class RelayCommand</summary>
// -----------------------------------------------------------------------

namespace Common.ViewModel
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Command implementation class
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// Action to execute
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// Condition to execute the action
        /// </summary>
        private readonly Predicate<object> canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">Action to execute</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">Action to execute</param>
        /// <param name="canExecute">Condition to execute the action</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }
        #endregion

        #region ICommand Members

        /// <summary>
        /// Inform about a change on the condition
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Condition to execute the action.
        /// </summary>
        /// <param name="parameter">Parameter of command</param>
        /// <returns>True if the command can be executed</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="parameter">Parameter of command</param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion
    }
}