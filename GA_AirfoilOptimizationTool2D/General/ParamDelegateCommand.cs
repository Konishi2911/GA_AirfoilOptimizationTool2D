using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GA_AirfoilOptimizationTool2D.General
{
    public class ParamDelegateCommand<T> : ICommand
    {
        private Action<T> execute;
        private Func<bool> canExecute;

        public ParamDelegateCommand(Action<T> execute, Func<bool> canExecute)
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

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

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
            this.execute((T)new object());
        }

        public bool CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }
    }
}
