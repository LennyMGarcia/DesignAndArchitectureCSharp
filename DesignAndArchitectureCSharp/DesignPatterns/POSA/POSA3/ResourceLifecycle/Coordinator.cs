using System;
using System.Linq;
using System.Collections.Generic;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceLifecycle
{

    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class ResourceLifecycleCoordinator
    {
        private readonly Dictionary<string, Resource> _resources = new Dictionary<string, Resource>();
        //permite que varios lectores accedan al recurso simultáneamente mientras se evita el acceso concurrente de escritores, o bien que un solo escritor tenga acceso exclusivo al recurso.
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public TResource Acquire<TResource>(string resourceId, Func<TResource> resourceFactory) where TResource : Resource
        {
            _lock.EnterWriteLock();
            try
            {
                //El recurso ya ha sido adquirido
                if (_resources.TryGetValue(resourceId, out Resource? existingResource))
                {
                    throw new InvalidOperationException($"Recurso {resourceId} ya adquirido");
                }

                TResource resource = resourceFactory();// cuando la funcion asociada se ejecute se crea el objeto, es una creacion lazy
                _resources.Add(resourceId, resource);
                return resource;
            }
            catch (Exception )
            {
                //al haber error se elimina el recurso
                _resources.Remove(resourceId);
                throw;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Release(string resourceId)
        {
            _lock.EnterWriteLock();
            try
            {
                if (!_resources.TryGetValue(resourceId, out Resource? resource))
                {
                    // El recurso ya ha sido eliminado, no hay nada que hacer
                    return;
                }

                if (resource.IsReleased)
                {
                    throw new InvalidOperationException($"El recurso {resourceId} ya se limpio");
                }

                try
                {
                    resource.Release();
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error limpiando el {resourceId}: {ex.Message}");
                }
                finally
                {
                    _resources.Remove(resourceId); // Eliminar el recurso de la lista
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        //cualquier tipo que se pase como TResource debe ser una subclase de Resource o Resource misma
        public TResource Get<TResource>(string resourceId) where TResource : Resource
        {
            _lock.EnterReadLock();
            try
            {
                if (!_resources.TryGetValue(resourceId, out Resource? resource))
                {
                    throw new InvalidOperationException($"Recurso {resourceId} no adquirido");
                }

                return (TResource)resource;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public abstract class Resource
    {
        public abstract void Release();
        public bool IsReleased { get; protected set; }
    }

    public class Printer : Resource
    {
        private bool _isPrinting;

        public void Print(string message)
        {
            _isPrinting = true;
            Console.WriteLine($"Imprimiendo: {message}");
        }

        public override void Release()
        {
            if (_isPrinting)
            {
                Console.WriteLine("Deteniendo la impresion");
                _isPrinting = false;
            }
            IsReleased = true;
        }
    }

    public class PrinterFactory
    {
        public Printer Create()
        {
            return new Printer();
        }
    }

}
