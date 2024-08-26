using System;
using System.Linq;
using System.Collections.Generic;

//
namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceLifecycle
{
    internal interface IResource<T>
    {
        T Get();
        void Release();
        bool IsValid { get; }
        bool IsAcquired { get; }
        event EventHandler<ResourceEventArgs> ResourceAcquired;
        event EventHandler<ResourceEventArgs> ResourceReleased;
        event EventHandler<ResourceEventArgs> ResourceInvalidated;
        void Invalidate();
    }

    public class RL_CachedResource<T>(Func<T> createResource) : IResource<T>
    {
        private readonly Func<T> _createResource = createResource;
        private readonly object _lock = new object();
        private T? _resource;
        private int _referenceCount;
        private bool _isReleased;
        private bool _isAcquired;
        private bool _isInvalidated;

        public T Get()
        {
            lock (_lock)
            {
                if (_isReleased)
                {
                    throw new InvalidOperationException("El recurso ha sido liberado");
                }
                else if (_isInvalidated)
                {
                    throw new InvalidOperationException("El recurso ha sido invalidado");
                }

                if (!_isAcquired)
                {
                    _resource = _createResource();
                    _isAcquired = true;
                    OnResourceAcquired(new ResourceEventArgs(_resource));
                }

                _referenceCount++;
                return _resource;
            }
        }

        public void Release()
        {
            lock (_lock)
            {
                if (_referenceCount > 0)
                {
                    _referenceCount--;
                }

                if (_referenceCount == 0)
                {
                    _isReleased = true;
                    _resource = default(T);
                    _isAcquired = false;
                    OnResourceReleased(new ResourceEventArgs(_resource));
                }
            }
        }

        public void Invalidate()
        {
            lock (_lock)
            {
                if (!_isInvalidated)
                {
                    _isInvalidated = true;
                    OnResourceInvalidated(new ResourceEventArgs(_resource));
                }
            }
        }

        public bool IsValid => !_isReleased && !_isInvalidated;
        public bool IsAcquired => _isAcquired;

        //Los eventos en este caso sirven para una futura implementacion, ya que el
        //cache debe de estar informado y logueado adecuadamente

        public event EventHandler<ResourceEventArgs> ResourceAcquired;
        public event EventHandler<ResourceEventArgs> ResourceReleased;
        public event EventHandler<ResourceEventArgs> ResourceInvalidated;

        protected virtual void OnResourceAcquired(ResourceEventArgs e)
        {
            ResourceAcquired?.Invoke(this, e);
        }

        protected virtual void OnResourceReleased(ResourceEventArgs e)
        {
            ResourceReleased?.Invoke(this, e);
        }

        protected virtual void OnResourceInvalidated(ResourceEventArgs e)
        {
            ResourceInvalidated?.Invoke(this, e);
        }
    }

    public class RL_ResourceCache<TKey, TValue>
    {
        private readonly Dictionary<TKey, IResource<TValue>> _cache = new Dictionary<TKey, IResource<TValue>>();
        private readonly object _lock = new object();


        private readonly Dictionary<TKey, bool> _invalidatedResources = new Dictionary<TKey, bool>();

        public TValue GetResource(TKey key, Func<TValue> createResource)
        {
            IResource<TValue> resource;
            lock (_lock)
            {
                if (_cache.TryGetValue(key, out resource))
                {
                    if (!resource.IsValid)
                    {
                        if (_invalidatedResources.TryGetValue(key, out bool isInvalidated) && isInvalidated)
                        {
                            throw new InvalidOperationException("El recurso ha sido invalidado");
                        }
                        else
                        {
                            resource = new RL_CachedResource<TValue>(createResource);
                            _cache[key] = resource; // Actualizar el recurso existente
                            return resource.Get();
                        }
                    }
                    else
                    {
                        return resource.Get();
                    }
                }
                else
                {
                    if (_invalidatedResources.TryGetValue(key, out bool isInvalidated) && isInvalidated)
                    {
                        throw new InvalidOperationException("El recurso ha sido invalidado");
                    }

                    resource = new RL_CachedResource<TValue>(createResource);
                    _cache.Add(key, resource);
                    _invalidatedResources[key] = true;
                    return resource.Get();
                }
                return default(TValue);
            }
        }


        public void ReleaseResource(TKey key)
        {
            lock (_lock)
            {
                IResource<TValue> resource;
                if (_cache.TryGetValue(key, out resource))
                {
                    //Se limpia y si no es valido se elimina
                    resource.Release();
                    if (!resource.IsValid)
                    {
                        _cache.Remove(key);
                    }
                }
            }
        }
        public void InvalidateResource(TKey key)
        {
            lock (_lock)
            {
                IResource<TValue> resource;
                if (_cache.TryGetValue(key, out resource))
                {
                    //Se invalida
                    resource.Invalidate();
                    _invalidatedResources[key] = false; // Establecer en false cuando se invalida el recurso
                }
            }
        }
    }

    public class ResourceEventArgs(object resource) : EventArgs
    {
        public object Resource { get; } = resource;
    }
}

