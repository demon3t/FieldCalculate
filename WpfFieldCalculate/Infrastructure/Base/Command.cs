using System;
using System.Windows.Input;

namespace WpfFieldCalculate.Infrastructure.Base
{
    /// <summary>
    /// Команда.
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Событие.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Проверка разрешения на выполнение команды.
        /// </summary>
        /// <param name="parameter">Параметры.</param>
        /// <returns>Результат проверки.</returns>
        public abstract bool CanExecute(object? parameter);

        /// <summary>
        /// Логика выполнения команда.
        /// </summary>
        /// <param name="parameter">Параметры.</param>
        public abstract void Execute(object? parameter);
    }
}
