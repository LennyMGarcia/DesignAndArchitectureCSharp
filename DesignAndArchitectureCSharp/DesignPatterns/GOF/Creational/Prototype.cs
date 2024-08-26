using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Creational
{
    public interface IPrototype<T>
    {
        T Clone();
    }
    public class Persona : IPrototype<Persona>
    {
        public string? Nombre { get; set; }
        public int Edad { get; set; }
        public decimal Salario { get; set; }
        public string? Direccion { get; set; }

        public Persona Clone()
        {
            return (Persona)this.MemberwiseClone();
            //es algo asi:
            /*var personaClonada = new Persona();
                personaClonada.Nombre = this.Nombre;
                personaClonada.Edad = this.Edad;
                personaClonada.Salario = this.Salario;
                personaClonada.Direccion = this.Direccion;
                return personaClonada;
            */
        }
    }
}
