using System;
using System.Collections.Generic;

namespace ReactiveVariables
{
    public class ReactiveProperty<T> : IReadOnlyReactiveProperty<T>
    {
        public event Action<T, T> Changed;

        private T _value;
        private IEqualityComparer<T> _comparer;

        public ReactiveProperty() : this(default(T)) { }

        public ReactiveProperty(T value) : this(value, EqualityComparer<T>.Default) { }

        public ReactiveProperty(T value, IEqualityComparer<T> comparer)
        {
            _value = value;
            _comparer = comparer;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    Changed?.Invoke(_value, value);
                    _value = value;
                }
            }
        }

        public void SetValueWithoutAction(T value)
        {
            _value = value;
        }
    }
}