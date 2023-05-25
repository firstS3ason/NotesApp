using Notes.App.Commands.Base;
using System;

namespace Notes.App.Commands
{
    internal class LambdaCommand : CommandBase
    {
        private readonly Func<object, bool> canExecute;
        private readonly Action<object> execute;
        public LambdaCommand(Func<object, bool> _canExecute, Action<object>? _execute)
        {
            canExecute = _canExecute;
            execute = _execute ?? throw new ArgumentNullException(nameof(execute));
        }
        public override bool CanExecute(object? param) => canExecute.Invoke(param);
        public override void Execute(object? param) => execute.Invoke(param);
    }
}
