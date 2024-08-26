using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA2.Concurrency
{
    public class LeaderFollower
    {
        private readonly BlockingCollection<string> _eventQueue = new BlockingCollection<string>();
        private readonly Thread[] _threads;
        private readonly object _lock = new object();
        private volatile int _leaderIndex = -1;
        private volatile bool _running;

        public LeaderFollower(int threadCount)
        {
            _threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                _threads[i] = new Thread(Run) { Name = $"Thread-{i}" };
            }
        }

        public void Start()
        {
            _running = true;
            _leaderIndex = 0; //Establecer el líder inicial
            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i].Start(i); //aqui se llama a Run con el indice del thread como parametro
            }
        }

        public void Stop()
        {
            _running = false;
            _eventQueue.CompleteAdding();

            lock (_lock)
            {
                Monitor.PulseAll(_lock);
            }

            foreach (var thread in _threads)
            {
                thread.Join();
            }
        }

        public void SignalEvent(string eventData)
        {
            _eventQueue.Add(eventData);
        }

        private void Run(object threadIndex)
        {
            int index = (int)threadIndex;

            while (_running)
            {
                string eventData;
                if (_leaderIndex == index)
                {
                    //Soy el lider
                    try
                    {
                        if (_eventQueue.TryTake(out eventData, Timeout.Infinite))
                        {
                            Console.WriteLine($"Lider {_leaderIndex} proceso evento: {eventData}");
                            //promocionar un nuevo loder
                            Interlocked.Increment(ref _leaderIndex);
                            _leaderIndex %= _threads.Length;
                            lock (_lock)
                            {
                                Monitor.PulseAll(_lock);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar evento: {ex.Message}");
                    }
                }
                else
                {
                    //soy seguidor
                    lock (_lock)
                    {
                        while (_leaderIndex != index && _running)
                        {
                            Monitor.Wait(_lock);
                        }
                    }
                }
            }
        }
    }
}