using System;
using System.Windows.Input;

namespace Project.ViewModel;

class RelayCommand : ICommand
{

    private readonly Action<object> _executeAct;
    private readonly Predicate<object> _canExecuteAct;
    public RelayCommand(Action<object> action)
    {
        _executeAct = action;
        _canExecuteAct = null;
    }
    public RelayCommand(Action<object> action, Predicate<object> predicate)
    {
        _executeAct = action;
        _canExecuteAct = predicate;
    }
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }


    public bool CanExecute(object? parameter)
    {
        if (_canExecuteAct == null)
            return true;
        else
            return _canExecuteAct(parameter);
    }

    public void Execute(object? parameter)
    {
        _executeAct(parameter);
    }
}
