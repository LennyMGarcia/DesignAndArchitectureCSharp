using System;
using System.Linq;
using System.Collections.Generic;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceLifecycle
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public interface IResource
    {
        void Initialize();
        void Finalize();
        void Release();
    }
    //tiene como restriccion que se pase un constructor sin parametros
    public class ResourceLifecycleManager<T>(Func<T> resourceFactory, Action<T> resourceInitializer, Action<T> resourceFinalizer, int maxResources) where T : IResource, new()
    {
        private readonly object _lock = new object();
        private readonly Dictionary<T, int> _resourceRefCount = new Dictionary<T, int>();
        private readonly Queue<T> _availableResources = new Queue<T>();
        private readonly Func<T> _resourceFactory = resourceFactory;
        private readonly Action<T> _resourceInitializer = resourceInitializer;
        private readonly Action<T> _resourceFinalizer = resourceFinalizer;
        private readonly int _maxResources = maxResources;

        //La idea es que se pueda obtene el recurso dos veces, si se requiere mas veces incrementa el contador
        public T AcquireResource()
        {
            lock (_lock)
            {
                T resource;
                if (_availableResources.TryDequeue(out resource))
                {
                    _resourceRefCount[resource] = 1;
                    resource.Initialize();
                    return resource;
                }
                else if (_resourceRefCount.Count < _maxResources)
                {
                    resource = _resourceFactory();
                    _resourceRefCount.Add(resource, 1);
                    resource.Initialize();
                    return resource;
                }
                else
                {
                    throw new InvalidOperationException("No hay recursos disponibles");
                }
            }
        }

        public void ReleaseResource(T resource)
        {
            lock (_lock)
            {
                if (_resourceRefCount.TryGetValue(resource, out int count))
                {
                    if (count == 1)
                    {
                        _resourceRefCount.Remove(resource);
                        _availableResources.Enqueue(resource);
                        resource.Finalize();
                    }
                    else
                    {
                        _resourceRefCount[resource] = count - 1;
                    }
                }
            }
        }

        public void CleanUp()
        {
            lock (_lock)
            {
                foreach (var resource in _resourceRefCount.Keys.ToList())
                {
                    resource.Finalize();
                    resource.Release();
                }
                _resourceRefCount.Clear();
                _availableResources.Clear();
            }
        }
    }

    public class YourResource : IResource
    {
        public void Initialize()
        {
            Console.WriteLine("Inicializando recurso...");
        }

        public void Finalize()
        {
            Console.WriteLine("Finalizando recurso...");
        }

        public void Release()
        {
            Console.WriteLine("Liberando recurso...");
        }
    }


}