using System;
using System.Linq;
using System.Collections.Generic;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA2.Concurrency
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public class P2_ThreadSpecificStorage<T>
    {
        private readonly ThreadLocal<T> _storage;

        public P2_ThreadSpecificStorage()
        {
            _storage = new ThreadLocal<T>(() => default(T));
        }

        public T? GetValue()
        {
            return _storage.Value;
        }

        public void SetValue(T value)
        {
            _storage.Value = value;
        }
    }

    public class P2_ThreadSpecificStorageFactory
    {
        private readonly ConcurrentDictionary<Type, object> _factories;

        public P2_ThreadSpecificStorageFactory()
        {
            _factories = new ConcurrentDictionary<Type, object>();
        }

        public P2_ThreadSpecificStorage<T> CreateStorage<T>()
        {
            return (P2_ThreadSpecificStorage<T>)_factories.GetOrAdd(typeof(T), _ => new P2_ThreadSpecificStorage<T>());
        }
    }
}

