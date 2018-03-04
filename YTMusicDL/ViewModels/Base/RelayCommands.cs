using System;
using System.Windows.Input;

namespace YTMusicDL
{
    class RelayCommand : ICommand
    {
        readonly Action<object> _methodAction;
        readonly Predicate<object> _verifyAction;

        public RelayCommand(Action<object> execute)
        {
            _methodAction = execute ?? throw new ArgumentNullException("execute");
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            : this(execute)
        {
            _verifyAction = canExecute;
        }
        
        public bool CanExecute(object parameter)
        {
            return _verifyAction == null ? true : _verifyAction(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _methodAction(parameter);
        }
    }
}
