using ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceRelease
{


    public class ConnectionEvictor
    {
        private readonly ConnectionPool _connectionPool;
        private readonly TimeSpan _inactivityTimeout;
        private readonly Timer _timer;

        public ConnectionEvictor(ConnectionPool connectionPool, TimeSpan inactivityTimeout)
        {
            //timer manejara el checkConnections que se encargara de inactivar la conexion
            _connectionPool = connectionPool;
            _inactivityTimeout = inactivityTimeout;
            _timer = new Timer(CheckConnections, null, _inactivityTimeout, _inactivityTimeout);
        }

        private void CheckConnections(object state)
        {
            var connectionsToCheck = _connectionPool.ToList();
            foreach (var connection in connectionsToCheck)
            {
                if (connection.LastActivity.Add(_inactivityTimeout) < DateTime.Now)
                {
                    _connectionPool.InactivateConnection(connection);
                }
            }
        }
    }
    //Busca manejar una serie de conexiones, El getEnumerator es necesario para el ICollection
    //Basicamente esta es una clase iterable
    public class ConnectionPool : ICollection<DatabaseConnection>
    {
        private readonly List<DatabaseConnection> _connections = new List<DatabaseConnection>();

        public int Count => _connections.Count;

        public bool IsReadOnly => false;

        public void Add(DatabaseConnection connection)
        {
            _connections.Add(connection);
        }

        public void Clear()
        {
            foreach (var connection in _connections)
            {
                connection.Dispose();
            }
            _connections.Clear();
        }

        public bool Contains(DatabaseConnection connection)
        {
            return _connections.Contains(connection);
        }

        public void CopyTo(DatabaseConnection[] array, int arrayIndex)
        {
            _connections.CopyTo(array, arrayIndex);
        }

        public IEnumerator<DatabaseConnection> GetEnumerator()
        {
            return _connections.GetEnumerator();
        }

        public bool Remove(DatabaseConnection connection)
        {
            return _connections.Remove(connection);
        }

        public void InactivateConnection(DatabaseConnection connection)
        {
            // El pool de conexiones es responsable de liberar la conexión
            ReleaseConnection(connection);
        }

        private void ReleaseConnection(DatabaseConnection connection)
        {
            connection.Dispose();
            _connections.Remove(connection);
        }


        //se usa system en ves de  IEnumerable<T> y IEnumerator<T> para evitar conflictos con las interfaces genericas
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class DatabaseConnection(string connectionString) : IDisposable
    {
        public string ConnectionString { get; set; } = connectionString;
        public DateTime LastActivity { get; set; } = DateTime.Now;

        public void ExecuteQuery(string query)
        {
            Console.WriteLine($"Ejecutando query '{query}' en la conexion {ConnectionString}");
            LastActivity = DateTime.Now;
        }

        public void Dispose()
        {
            Console.WriteLine($"Cerrando coneciones de la base de datos {ConnectionString}");
            // Liberar recursos de la conexion aqui
        }
    }

}
