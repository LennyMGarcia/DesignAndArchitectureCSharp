using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;


namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA2.Concurrency
{
    public interface P2_MessageQueue
    {
        void Enqueue(P2_Message message);
        bool TryDequeue(out P2_Message message);
        P2_Message Dequeue();
    }
    public class P2_MessageQueueImpl : P2_MessageQueue
    {
        private ConcurrentQueue<P2_Message> queue = new ConcurrentQueue<P2_Message>();

        public void Enqueue(P2_Message message)
        {
            queue.Enqueue(message);
        }

        public bool TryDequeue(out P2_Message message)
        {
            return queue.TryDequeue(out message);
        }

        public P2_Message Dequeue()
        {
            P2_Message message;
            if (TryDequeue(out message))
            {
                return message;
            }
            else
            {
                throw new InvalidOperationException("La cola esta vacia");
            }
        }
    }
    public interface P2_ActiveObject
    {
        void Start();
        void Stop();
        void Enqueue(P2_Message message);
        void WaitForCompletion();
    }
    public class P2_ActiveObjectImpl(P2_MessageQueue messageQueue) : P2_ActiveObject
    {
        private P2_MessageQueue messageQueue = messageQueue;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private Task processingTask;
        private Thread thread;
        private bool running;
        private object lockObject = new object();

        public void Start()
        {
            processingTask = Task.Run(() => Run(cancellationTokenSource.Token));
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            processingTask.Wait();
        }

        public void Enqueue(P2_Message message)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                throw new InvalidOperationException("Active object esta detenido, no puede encolar mas mensajes");
            }
            messageQueue.Enqueue(message);
            lock (lockObject)
            {
                Monitor.Pulse(lockObject); // Notificar que hay un nuevo mensaje
            }
        }

        private TaskCompletionSource<bool> completionTaskSource = new TaskCompletionSource<bool>();

        public void WaitForCompletion()
        {
            completionTaskSource.Task.Wait(); 
            while (messageQueue.TryDequeue(out _)) { } 
            completionTaskSource.TrySetResult(true); 
        }

        private void Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                P2_Message message;
                lock (lockObject)
                {
                    while (!messageQueue.TryDequeue(out message) && !cancellationToken.IsCancellationRequested)
                    {
                        Monitor.Wait(lockObject); //esperar hasta que haya un mensaje
                    }
                }

                if (message != null)
                {
                    message.Process();
                }
                else
                {
                    break; 
                }
            }
            completionTaskSource.TrySetResult(true); //notificar que se ha completado el procesamiento
        }
    }

    public abstract class P2_Message
    {
        public abstract void Process();
    }
    public class P2_OrderMessage(string item) : P2_Message
    {
        private string item = item;

        public override void Process()
        {
            Console.WriteLine($"Procesando orden: {item}");
            //simular el procesamiento de la orden
            Thread.Sleep(2000);
            Console.WriteLine($"Orden completada: {item}");
        }
    }


}