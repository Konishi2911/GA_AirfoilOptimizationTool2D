using System;
using System.Windows.Input;

namespace GA_AirfoilOptimizationTool2D
{
    class OptConfigDelegateCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public OptConfigDelegateCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Inquire whether be executable condition.
        /// </summary>
        /// <returns>If executable condition, ture</returns>
        public bool CanExecute()
        {
            return this.canExecute();
        }

        /// <summary>
        /// Execute the Command.
        /// </summary>
        public void Execute()
        {
            this.execute();
        }

        public bool CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        public void Execute(object parameter)
        {
            this.Execute();
        }
    }
}
