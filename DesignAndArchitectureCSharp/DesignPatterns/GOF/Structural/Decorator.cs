using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Forma parte del patron estructural pero los decoradores se puede hacer con atributos
namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural
{
    //define que se puede alterar con el decorador
    public abstract class CoffeeComponent
    {
        public abstract string Description();
        public abstract double Cost();
    }

    class SimpleCoffee : CoffeeComponent
    {
        public override string Description()
        {
            return "Simple Coffee";
        }

        public override double Cost()
        {
            return 1.0;
        }
    }

    abstract class CoffeeDecorator(CoffeeComponent coffee) : CoffeeComponent
    {
        protected CoffeeComponent _coffee = coffee;

        // el decorador delega todo el trabajo a los componentes envueltos
        public override string Description()
        {
            if (this._coffee != null)
            {
                return this._coffee.Description();
            }
            else
            {
                return string.Empty;
            }
        }
        //En caso de que no haya usualmente se le ponen valores default
        public override double Cost()
        {
            if (this._coffee != null)
            {
                return this._coffee.Cost();
            }
            else
            {
                return 0.0;
            }
        }
    }
    //Decoradores concretos
    class MochaDecorator(CoffeeComponent coffee) : CoffeeDecorator(coffee)
    {

        // puede tener implementacion unica pero me gusta agregar impuestos, ok no
        public override string Description()
        {
            return $"{base.Description()}, Mocha";
        }

        public override double Cost()
        {
            return base.Cost() + 0.5;
        }
    }

    class WhippedCreamDecorator(CoffeeComponent coffee) : CoffeeDecorator(coffee)
    {
        public override string Description()
        {
            return $"{base.Description()}, Whipped Cream";
        }

        public override double Cost()
        {
            return base.Cost() + 0.25;
        }
    }
}
