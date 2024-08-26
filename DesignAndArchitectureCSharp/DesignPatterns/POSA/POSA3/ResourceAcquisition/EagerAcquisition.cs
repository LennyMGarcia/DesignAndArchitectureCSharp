using System;
using System.Linq;
using System.Collections.Generic;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceAcquisition
{
    public class EagerAcquisition<T> where T : class, IDisposable
    {
        private readonly T _resource;

        public EagerAcquisition()
        {
            _resource = CreateResource();
        }

        protected virtual T CreateResource()
        {
            //Aunque facilita la idea de creacion y de entendimiento
            //no se recomienda el uso de IActivator por ser inflexible
            return Activator.CreateInstance<T>();
        }

        public T Get()
        {
            return _resource;
        }

        public void Dispose()
        {
            Dispose(true);
            // despues de limpiar evitar que el finalizador se llame innecesariamente
            GC.SuppressFinalize(this);
        }
        // se asegura de que se libere el recurso
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //se libera el recurso
                _resource.Dispose();
            }
        }

        ~EagerAcquisition()
        {
            //Esta imcompleto pero ayuda a mantener el dispose en false en caso de olvido
            Dispose(false);
        }
    }
}
