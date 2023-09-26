using System;
using WpfFieldCalculate.Infrastructure.Base;

namespace WpfFieldCalculate.Infrastructure
{
    /// <summary>
    /// Выполняемая команда.
    /// </summary>
    public class RelayCommand : Command
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="execute">Логика выполнения команды.</param>
        /// <param name="canExecute">Разрешение на выполнения команды.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            #region Валидация

            if (execute is null)
            {
                throw new ArgumentNullException(nameof(execute), "Передан пустой обработчик события.");
            }

            #endregion

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Проверка разрешения на выполнение команды.
        /// </summary>
        /// <param name="parameter">Параметры.</param>
        /// <returns>Результат проверки.</returns>
        public override bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter ?? false) ?? true;

        /// <summary>
        /// Логика выполнения команды.
        /// </summary>
        /// <param name="parameter">Параметры.</param>
        public override void Execute(object? parameter) => _execute(parameter ?? false);
    }
}
