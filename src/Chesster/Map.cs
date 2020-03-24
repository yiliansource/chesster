using System.Collections.Generic;

namespace Chesster
{
    public class Map<T1, T2>
    {
        public T2 this[T1 v]
        {
            get => _forward[v];
            set => Add(v, value);
        }
        public T1 this[T2 v]
        {
            get => _reverse[v];
            set => Add(value, v);
        }

        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }
    }
}
