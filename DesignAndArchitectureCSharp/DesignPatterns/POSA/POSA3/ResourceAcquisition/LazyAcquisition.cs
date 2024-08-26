using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceRelease
{

    public class LazyAcquisition<T> where T : class, IDisposable
    {
        //Hara que se inicialice la instancia solo cuando se llame la clase get
        private readonly Lazy<T> _lazyResource;
        private readonly object _lock = new object();

        public LazyAcquisition()
        {
            _lazyResource = new Lazy<T>(() =>
            {
                try
                {
                    return Activator.CreateInstance<T>();
                }
                catch (Exception ex)
                {
                    throw new LazyInitializationException("Error al inicializar Lazy resource", ex);
                }
            });
        }

        public T Get()
        {
            //inicializador
            return _lazyResource.Value;
        }

        public void Dispose()
        {
            //Solo un hilo ejecutara el dispose, varios hilos no puede hacerlo porque da problemas
            lock (_lock)
            {
                if (_lazyResource.IsValueCreated)
                {
                    _lazyResource.Value.Dispose();
                }
            }
        }
    }

    public class LazyInitializationException(string message, Exception innerException) : Exception(message, innerException)
    {
    }

    public class ExpensiveResource : IDisposable
    {
        private bool _disposed = false;
        private readonly object _lock = new object();

        public ExpensiveResource()
        {
            Console.WriteLine("Recurso pesado creado");
        }

        public void DoSomething()
        {
            Console.WriteLine("Hago algo ahaha");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Libera los recursos administrados aqui (OJO)
                //ejemplo, libera objetos que implementan IDisposable
            }

            // Libera los recursos no administrados aqui (OJO)
            //ejemplo, cierra conexiones a bases de datos, archivos, etc.
            Console.WriteLine("ExpensiveResource disposed");

            _disposed = true;
        }

        ~ExpensiveResource()
        {
            Dispose(false);
        }
    }
}