using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA2.Concurrency
{

    // Clase que representa el sincronizador
    //se reciben en orden secuencial, pero se procesan en paralelo. 
    public class P2_ConcurrencyGate(int maxConcurrency)
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(maxConcurrency);
        private readonly P2_TaskProcessor _taskProcessor = new P2_TaskProcessor();

        public async Task ProcessRequestAsync(int request)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!IsValidRequest(request))
                {
                    Console.WriteLine($"Solicitud {request} no valida");
                    return;
                }

                await _taskProcessor.ProcessRequestAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar la solicitud {request}: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private bool IsValidRequest(int request)
        {
            return request > 0;
        }
    }

    public class P2_TaskProcessor
    {
        public async Task ProcessRequestAsync(int request)
        {
            Console.WriteLine($"Solicitud {request} recibida");
            //procesamiento asincronico: 
            await Task.Delay(1000);
            Console.WriteLine($"Solicitud {request} procesada asincronicamente");
        }
    }

}