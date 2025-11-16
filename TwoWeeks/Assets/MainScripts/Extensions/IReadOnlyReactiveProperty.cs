using System;

namespace ReactiveVariables
{
    public interface IReadOnlyReactiveProperty<T>
    {
        public event Action<T, T> Changed;
    }
}
