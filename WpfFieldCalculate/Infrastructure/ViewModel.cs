using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfFieldCalculate.Infrastructure
{
    /// <summary>
    /// View модель.
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод вызываемый при изменении свойства.
        /// </summary>
        /// <param name="PropertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Установить значение свойства.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="field">Изменяемое поле.</param>
        /// <param name="value">Значение.</param>
        /// <param name="PropertyName">Имя свойства.</param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
