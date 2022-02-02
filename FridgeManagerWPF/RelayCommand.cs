using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FridgeManagerWPF
{
    public class RelayCommand : ICommand
    {
        #region ICommand interface
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
        #endregion

        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> _execute, Predicate<object> _canExecute)
        {
            if (_execute == null)
                throw new ArgumentNullException("Execute");

            execute = _execute;
            canExecute = _canExecute;
        }

        public RelayCommand(Action<object> _execute) : this(_execute, null) { }
    }
}
