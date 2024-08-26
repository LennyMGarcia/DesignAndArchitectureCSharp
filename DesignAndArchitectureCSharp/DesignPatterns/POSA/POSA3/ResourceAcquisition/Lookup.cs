using System;
using System.Collections.Generic;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceAcquisition
{
    //Su principal ventaja es la busqueda por id, implementa un Func que servira para buscar por id
    public class WorkspaceSessionLookup(Func<string, WorkspaceSession> sessionFactory)
    {
        private readonly Dictionary<string, WorkspaceSession> _sessions = new Dictionary<string, WorkspaceSession>();
        private readonly Func<string, WorkspaceSession> _sessionFactory = sessionFactory;
        private readonly object _lock = new object(); // objeto de bloqueo

        public WorkspaceSession GetSession(string sessionId)
        {
            WorkspaceSession session;
            lock (_lock) // bloqueo para asegurar thread-safety
            {
                if (_sessions.TryGetValue(sessionId, out session))
                {
                    return session;
                }

                session = _sessionFactory(sessionId);
                _sessions.Add(sessionId, session);
            }
            return session;
        }
    }

    public class WorkspaceSession : IDisposable
    {
        public string? SessionId { get; set; }

        public void Start()
        {
            Console.WriteLine("Comenzando workspace...");
        }

        public void Stop()
        {
            Console.WriteLine("Parando seccion del  workspace...");
        }

        public void Dispose()
        {
            Stop();
        }
    }
}