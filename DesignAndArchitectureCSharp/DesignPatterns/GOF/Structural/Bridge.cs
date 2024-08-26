using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//permite separar la logica de negocio de la implementacion especifica 
//De esta manera, la abstraccion (Vehicle) puede delegar el trabajo a la implementacion concreta (IEngine)
//sin conocer los detalles de la implementacion
//De esta manera, la abstracción se enfoca en la logica de negocio y la implementación concreta se enfoca en la implementacion especifica.
namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural
{
    // Abstraction
    public abstract class Vehicle(IEngine engine)
    {
        protected IEngine _engine = engine;

        public virtual string StartEngine()
        {
            return "Vehicle: comenzando la maquinaria con:\n" + _engine.Start();
        }

        public virtual string Accelerate()
        {
            return "Vehicle: accelerando con:\n" + _engine.Accelerate();
        }
    }

    // Extended Abstraction
    public class Car(IEngine engine) : Vehicle(engine)
    {
        public override string StartEngine()
        {
            return "Car: comenzando la maquinaria con:\n" + base._engine.Start();
        }

        public override string Accelerate()
        {
            return "Car: acelerando con:\n" + base._engine.Accelerate();
        }
    }

    public class Truck(IEngine engine) : Vehicle(engine)
    {
        public override string StartEngine()
        {
            return "Truck: comenzando la maquinaria con:\n" + base._engine.Start();
        }

        public override string Accelerate()
        {
            return "Truck: acelerando con:\n" + base._engine.Accelerate();
        }
    }

    // Implementation
    public interface IEngine
    {
        string Start();
        string Accelerate();
    }

    // Concrete Implementation
    public class GasolineEngine : IEngine
    {
        public string Start()
        {
            return "GasolineEngine: gritando con fuerza.\n";
        }

        public string Accelerate()
        {
            return "GasolineEngine: acelerando con con mucha fuerza.\n";
        }
    }

    public class ElectricEngine : IEngine
    {
        public string Start()
        {
            return "ElectricEngine: prendiendo silenciosamente.\n";
        }

        public string Accelerate()
        {
            return "ElectricEngine: Acelerando suavemente (No es promocion).\n";
        }
    }
}
