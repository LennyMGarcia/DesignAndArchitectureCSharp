using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Timers;



namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceLifecycle
{

    // Interfaz para los recursos
    public interface IPosa3Resource
    {
        void Initialize();
        void Release();
    }

    public class MiRecurso : IPosa3Resource
    {
        public void Initialize()
        {
            Console.WriteLine("Inicializando MiRecurso...");
        }

        public void Release()
        {
            Console.WriteLine("Liberando MiRecurso...");
        }
    }
    // Implementacion basica de un recurso
    public class Posa3Resource : IPosa3Resource
    {
        public Guid Id { get; set; }

        public void Initialize()
        {
            Console.WriteLine("Inicializando recurso...");
        }

        public void Release()
        {
            Console.WriteLine("Liberando recurso...");
        }
    }

    public class ResourceReleasedEventArgs(IPosa3Resource resource) : EventArgs
    {
        public IPosa3Resource Resource { get; private set; } = resource;
    }

    // Wrapper alrededor de la clase Posa3Resource
    //Funciona como un proxy, aqui se puede agregar manejador de logs o restricciones
    internal class Posa3ResourceProxy(IPosa3Resource resource) : IPosa3Resource
    {
        internal Guid Id { get; set; }
        internal readonly IPosa3Resource _resource = resource;
        internal DateTime _lastAccessed;

        public void Initialize()
        {
            _resource.Initialize();
            _lastAccessed = DateTime.Now;
        }

        public void Release()
        {
            _resource.Release();
        }

        public DateTime LastAccessed
        {
            get { return _lastAccessed; }
        }
    }

    // Clase que gestiona el pool de recursos
    public class Posa3ResourcePool<T> where T : IPosa3Resource, new()
    {
        private readonly ConcurrentQueue<Posa3ResourceProxy> _resources;
        private readonly int _maxResources;
        private readonly int _minResources;
        private readonly TimeSpan _evictionTimeout;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private int _resourcesInUse;
        private Dictionary<Guid, Posa3ResourceProxy> _resourceMap = new Dictionary<Guid, Posa3ResourceProxy>();
        private readonly System.Timers.Timer _evictionTimer;

        public Posa3ResourcePool(int maxResources, int minResources, TimeSpan evictionTimeout)
        {
            //Se usa una cola concurrente debido al manejo concurrente de tiempo y recursos
            //Sirve mucho para evitar condiciones de carrera y sincronizar la cola
            _resources = new ConcurrentQueue<Posa3ResourceProxy>();
            _maxResources = maxResources;
            _minResources = minResources;
            _evictionTimeout = evictionTimeout;
            //se agrega el evict cada ves por cada recurso, autoreset en false por motivo de depuracion
            //Al final se inicia
            _evictionTimer = new System.Timers.Timer(_evictionTimeout.TotalHours);
            //Suscripcion al evento
            _evictionTimer.Elapsed += Evict;
            _evictionTimer.AutoReset = false;
            _evictionTimer.Start();
        }

        public void Add(T resource)
        {
            _lock.EnterWriteLock();
            try
            {
                //Se agrega y se encola el Wrapper
                var wrapper = new Posa3ResourceProxy(resource);
                wrapper.Id = Guid.NewGuid();
                _resources.Enqueue(wrapper);
                _resourceMap.Add(wrapper.Id, wrapper);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public IPosa3Resource Checkout()
        {
            _lock.EnterReadLock();
            try
            {
                //Devuelve un recurso disponible en la cola
                if (_resources.TryDequeue(out var resource))
                {
                    resource._lastAccessed = DateTime.Now;
                    return resource;
                }
                //Se crea uno pero de tipo generico y no agregado
                else if (_resources.Count < _maxResources)
                {
                    var newResource = CreateResource();
                    newResource.Initialize();
                    _resourceMap.Add(newResource.Id, newResource);
                    _resourcesInUse++;
                    return newResource;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        private Posa3ResourceProxy CreateResource()
        {
            //se crea un recurso de forma <T> y se le agrega el Guid
            var resource = new Posa3ResourceProxy((T)Activator.CreateInstance(typeof(T)));
            resource.Id = Guid.NewGuid();
            return resource;
        }

        //Agrega el recurso a la cola
        public void Checkin(IPosa3Resource resource)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_resources.Count < _maxResources)
                {
                    var wrapper = new Posa3ResourceProxy(resource);
                    _resources.Enqueue(wrapper);
                    _resourcesInUse--;
                }
                else
                {

                    resource.Release();
                    _resourcesInUse--;
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        //Evict que se puede usar para un Timer
        public void EvictTimerCallback(object state)
        {
            Evict();
        }

        private void Evict(object sender, ElapsedEventArgs e)
        {
            Evict();
        }

        private DateTime _lastEviction;

        public void Evict()
        {

            _lock.EnterWriteLock();
            try
            {
                var now = DateTime.Now;
                var timeout = now - _evictionTimeout;
                _lastEviction = now;
                //Elimina los recursos despues de un tiempo determinaddo
                while (_resources.TryPeek(out var resource))
                {
                    if (resource.LastAccessed < timeout)
                    {
                        Console.WriteLine($"resource id: {resource.Id}");
                        _resources.TryDequeue(out _);
                        resource.Release();
                        _resourceMap.Remove(resource.Id);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public int ResourcesAvailable
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _resources.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public int ResourcesInUse
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _resourcesInUse;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public event EventHandler<ResourceReleasedEventArgs> ResourceReleased;
        //Se puede agregar mas manejo a los eventos suscritos al eliminar, y se puede agregar
        //Dependencias en recursos 

        protected virtual void OnResourceReleased(ResourceReleasedEventArgs e)
        {
            ResourceReleased?.Invoke(this, e);
        }

        public IPosa3Resource? GetResource(Guid resourceId)
        {
            _lock.EnterReadLock();
            try
            {
                //Retorn el recurso
                if (_resourceMap.TryGetValue(resourceId, out var resource))
                {
                    return resource;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void ReleaseAllResources()
        {
            //Se eliminan todos los recursos e informacion
            _lock.EnterWriteLock();
            try
            {
                while (_resources.TryDequeue(out var resource))
                {

                    Console.WriteLine($"resource id: {resource.Id}");
                    resource.Release();
                    _resourceMap.Remove(resource.Id);
                }
                _resourcesInUse = 0;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}

