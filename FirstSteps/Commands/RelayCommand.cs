using FirstSteps.Commands.Base;
using System;
using System.Threading.Tasks;

namespace FirstSteps.Commands
{
    internal class RelayCommand : BaseCommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _executeAsync;
        private readonly Func<object, Task<bool>> _canExecuteAsync;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) 
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Func<object, Task> executeAsync, Func<object, Task<bool>> canExecuteAsync = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecuteAsync = canExecuteAsync;
        }

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => _execute(parameter);

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
            else
            {
                await _executeAsync(parameter);
            }
        }

        public override async Task<bool> CanExecuteAsync(object? parameter)
        {
            if (_canExecuteAsync != null)
            {
                return await _canExecuteAsync(parameter);
            }
            else if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            else
            {
                return true;
            }
        }
    }
}
