using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFPhonebook.Command
{
    public class RelayCommand : ICommand
    {
        Action<object> ExecuteMethod;
        Func<object, bool> CanExecuteMethod;
        bool CanExecuteCache;

        public RelayCommand(Action<object> ExecuteMethod, Func<object, bool> CanExecuteMethod,bool CanExecuteCache)
        {
            this.ExecuteMethod = ExecuteMethod;
            this.CanExecuteMethod = CanExecuteMethod;
            this.CanExecuteCache = CanExecuteCache;
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if(!CanExecuteCache)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (!CanExecuteCache)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return (CanExecuteMethod(parameter));
        }

        public void Execute(object parameter)
        {
            ExecuteMethod(parameter);
        }
    }
}
