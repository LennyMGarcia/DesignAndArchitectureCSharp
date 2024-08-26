using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Creational
{
    public class Singleton_SystemConfiguration
    {
        //es la parte esencial del patrom, hace que se devuelva siempre la misma instancia
        private static Singleton_SystemConfiguration? _instance;
        private static readonly object _lock = new();

        private string _databaseServer;
        private readonly string _databaseUsername;
        private readonly string _databasePassword;

        private Singleton_SystemConfiguration()
        {
            _databaseServer = "localhost";
            _databaseUsername = "root";
            _databasePassword = "password";
        }

        public static Singleton_SystemConfiguration Instance
        {
            get
            {
                //se utiliza para el doble chequeo de la instancia
                Singleton_SystemConfiguration? instance = _instance;
                //si no hay instancia inicializada
                if (instance == null)
                {
                    //bloquea los multihilos e inicializa la instancia
                    //evita que varios hilos ejecuten el bloque, el _lock marca territorio para evitar esto
                    lock (_lock)
                    {
                        instance = _instance;//segunda verificacion
                        if (instance == null)
                        {
                            //se crea la clase y debido a que es static se comparte
                            instance = new Singleton_SystemConfiguration();
                            _instance = instance;
                        }
                    }
                }
                //si ya esta inicializada siempre va aqui
                return instance;
            }
        }

        //se usa para inicializar
        public void Initialize(string databaseServer)
        {
            DatabaseServer = databaseServer;
        }

        public string DatabaseServer
        {
            get { return _databaseServer; }
            private set { _databaseServer = value; }
        }

        public string DatabaseUsername
        {
            get { return _databaseUsername; }
        }

        public string DatabasePassword
        {
            get { return _databasePassword; }
        }
    }
}
