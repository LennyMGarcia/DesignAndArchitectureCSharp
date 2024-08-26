using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    // Strategy Interface
    public interface ICompressionAlgorithm
    {
        void Compress(string data);
    }

    // Concrete Strategy 1: ZipCompression
    public class ZipCompression : ICompressionAlgorithm
    {
        public void Compress(string data)
        {
            Console.WriteLine($"Realizando compresion usando ZipCompression: {data}");
        }
    }

    // Concrete Strategy 2: RarCompression
    public class RarCompression : ICompressionAlgorithm
    {
        public void Compress(string data)
        {
            Console.WriteLine($"Realizando compresion usando RarCompression: {data}");
        }
    }

    // Context
    public class DataCompressor(ICompressionAlgorithm algorithm)
    {
        private ICompressionAlgorithm _algorithm = algorithm;

        //Ponerle funcionalidad de que pueda cambiar de algoritmo mientras ejecuta
        public void SetAlgorithm(ICompressionAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public void CompressData(string data)
        {
            _algorithm.Compress(data);
        }
    }
}
