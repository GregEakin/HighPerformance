using System.Threading;

namespace HighPerformanceTests.thread
{
    public class SimpleFuture<T>
    {
        private readonly object _lock = new object();
        private object _value;

        public SimpleFuture()
        {
            _value = this;
        }

        public SimpleFuture(T value)
        {
            _value = value;
        }

        public virtual T Value
        {
            get
            {
                lock (_lock)
                {
                    //while (_value == this)
                    //    Monitor.Wait(_lock);
                    return (T)_value;
                }
            }
            set
            {
                lock (_lock)
                {
                    if (_value != this) return;
                    _value = value;
                    //Monitor.PulseAll(_lock);
                }
            }
        }

        public bool Set
        {
            get
            {
                lock (_lock)
                {
                    return _value != this;
                }
            }
        }
    }
}