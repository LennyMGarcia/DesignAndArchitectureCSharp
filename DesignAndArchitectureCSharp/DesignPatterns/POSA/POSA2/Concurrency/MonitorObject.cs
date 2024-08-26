using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

//debug se usa para pruebas ya que el codigo es corto
namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA2.Concurrency
{
    public class ResourceGuard
    {
        private readonly object _mutex = new object();
        private int _waiters = 0;
        private bool _signal = false;
        private int _signalCount = 0;
        private bool _isAvailable = true;

        public void EnterResource()
        {
            lock (_mutex)
            {
                while (!_isAvailable)
                {
                    Monitor.Wait(_mutex);
                }
                _isAvailable = false;
            }
        }

        public void ExitResource()
        {
            lock (_mutex)
            {
                _isAvailable = true;
                Monitor.Pulse(_mutex);
            }
        }

        public void WaitForResource()
        {
            lock (_mutex)
            {
                _waiters++;
                Debug.Assert(_waiters > 0);
                Monitor.Wait(_mutex);
                _waiters--;
                Debug.Assert(_waiters >= 0);
            }
        }

        public void SignalResource()
        {
            lock (_mutex)
            {
                _signalCount++;
                Monitor.Pulse(_mutex);
            }
        }

        public void SignalAllResources()
        {
            lock (_mutex)
            {
                _signalCount = _waiters;
                Monitor.PulseAll(_mutex);
            }
        }

        public bool TryEnterResource(int timeout)
        {
            lock (_mutex)
            {
                if (_waiters > 0 || _signalCount > 0)
                {
                    if (!Monitor.Wait(_mutex, timeout))
                    {
                        return false;
                    }
                }
                _waiters++;
                Debug.Assert(_waiters > 0);
                return true;
            }
        }

        public bool TryEnterResource(TimeSpan timeout)
        {
            lock (_mutex)
            {
                if (_waiters > 0 || _signalCount > 0)
                {
                    if (!Monitor.Wait(_mutex, timeout))
                    {
                        return false;
                    }
                }
                _waiters++;
                Debug.Assert(_waiters > 0);
                return true;
            }
        }
    }
}





