using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural
{
    public interface IDataAccess
    {
        void GetData();
    }

    public class RealDataAccess : IDataAccess
    {
        public void GetData()
        {
            Console.WriteLine("RealDataAccess: Obtiniendo datos de acceso....");
        }
    }

    public class DataAccessProxy(RealDataAccess realDataAccess) : IDataAccess
    {
        private RealDataAccess _realDataAccess = realDataAccess;

        // aqui se le puede poner comprobadores de acceso y logging, es lo que permite el proxy
        public void GetData()
        {
            if (CheckAccess())
            {
                _realDataAccess.GetData();
                LogAccess();
            }
        }

        private bool CheckAccess()
        {
            Console.WriteLine("DataAccessProxy: Comprobando acceso a la base de datos...");
            return true;
        }

        private void LogAccess()
        {
            Console.WriteLine("DataAccessProxy: Logueando acceso a la base de datos");
        }
    }

    public class Client
    {
        public void ClientCode(IDataAccess dataAccess)
        {
            dataAccess.GetData();
        }
    }
}
