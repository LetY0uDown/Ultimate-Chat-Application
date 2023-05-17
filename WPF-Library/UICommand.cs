using System;
using System.Windows.Input;

namespace WPFLibrary;

public class UICommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool>? _canExecute = default;

    public UICommand (Action<object> execute)
    {
        ArgumentNullException.ThrowIfNull(execute);

        _execute = execute;
    }

    public UICommand (Action<object> execute, Func<object, bool> canExecute) : this(execute)
        => _canExecute = canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute (object? parameter)
    {
        return _canExecute is null || _canExecute(parameter!);
    }

    public void Execute (object? parameter)
    {
        _execute(parameter!);
    }
}