using System;
using System.Linq;
using System.Collections.Generic;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceRelease
{
    public interface ICacheLease : IDisposable
    {
        ICache Cache { get; }
        bool IsValid { get; }
    }
    //Cache lease se encarga de Hacer contratos, por eso recibe un cache y su accion de liberacion
    public class CacheLease(ICache cache, Action<ICache> releaseAction) : ICacheLease
    {
        private readonly ICache _cache = cache;
        private readonly Action<ICache> _releaseAction = releaseAction;
        private bool _isReleased = false;

        public ICache Cache => _cache;

        public bool IsValid => !_isReleased;

        public void Dispose()
        {
            Release();
        }

        private void Release()
        {
            if (_isReleased) return;
            _isReleased = true;
            _releaseAction(_cache);
        }
    }
    //cache pool se encarga de crear y adquirir los contratos, tambien maneja el almacenamiento
    //aqui se separan responsabilidades
    public class CachePool(Func<ICache> createCache, Action<ICache> releaseCache)
    {
        private readonly Func<ICache> _createCache = createCache;
        private readonly Action<ICache> _releaseCache = releaseCache;

        public ICacheLease Acquire()
        {
            var cache = _createCache();
            return new CacheLease(cache, _releaseCache);
        }
    }

    public interface ICache : IDisposable
    {
        void Add(string key, string value);
        string Get(string key);
    }
    //Memory cache se encarga de su almacenamiento
    public class MemoryCache : ICache
    {
        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
        private bool _isDisposed = false;

        public void Add(string key, string value)
        {
            if (_isDisposed) throw new ObjectDisposedException("MemoryCache");
            _cache[key] = value;
        }

        public string Get(string key)
        {
            if (_isDisposed) throw new ObjectDisposedException("MemoryCache");
            return _cache.TryGetValue(key, out var value) ? value : null;
        }

        public void Dispose()
        {
            lock (this)
            {
                if (_isDisposed) return;
                _cache.Clear();
                _isDisposed = true;
            }
        }
    }
}