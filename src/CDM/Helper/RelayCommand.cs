using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CDM.Helper
{
    public class RelayCommand : ICommand
    {
        #region :: Variables ::
        private Action<object> execute;
        private Func<object, bool> canExecute;
        private Action recentItemSort;
        #endregion

        #region :: Constructor ::
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action recentItemSort)
        {
            this.recentItemSort = recentItemSort;
        }
        #endregion

        #region :: Event Handler ::
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #region :: Methods ::
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }
        public void Execute(object parameter)
        {
            execute(parameter);
        }
        #endregion
    }
}
