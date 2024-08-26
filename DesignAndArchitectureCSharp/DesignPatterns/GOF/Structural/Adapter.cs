using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural
{
    public interface ILogger
    {
        void WriteLog(string message);
    }

    public interface IWriter
    {
        void Write(string message);
    }

    public class LoggerAdapter(IWriter writer) : ILogger
    {
        //se inicializara con el adapter y luego podra usar sus metodos
        private IWriter _writer = writer;

        public void WriteLog(string message)
        {
            _writer.Write(message);
        }
    }

    public class Logger : ILogger
    {
        public void WriteLog(string message)
        {
            // Escribir registro en un archivo
            Console.WriteLine($"Escribiendo registro en archivo: {message}");
        }
    }

    public class ConsoleWriter
    {
        public void Write(string message)
        {
            Console.WriteLine($"Escribiendo registro en consola: {message}");
        }
    }

    public class TTY2Writer : IWriter
    {
        public void Write(string message)
        {
            // Escribir registro en un dispositivo TTY2
            Console.WriteLine($"Escribiendo registro en TTY2: {message}");
        }
    }


}
