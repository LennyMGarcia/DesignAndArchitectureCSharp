using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural
{
    public class Computer
    {
        public void TurnOn() { Console.WriteLine("La computadora esta prendiendo..."); }
        public void TurnOff() { Console.WriteLine("La computadora se esta apagando..."); }
    }

    public class Projector
    {
        public void TurnOn() { Console.WriteLine("El proyector esta prendiendo..."); }
        public void TurnOff() { Console.WriteLine("El proyector se esta apagando..."); }
    }

    public class Screen
    {
        public void Down() { Console.WriteLine("La pantalla esta yendo hacia abajo..."); }
        public void Up() { Console.WriteLine("La pantalla esta yendo hacia arriba..."); }
    }

    // Facade class
    //en ves de ejecutar todos esos modulos como componentes separados, se puede manejar todo como uno solo
    // puedes hacer esto mas complejo utilizando interfaces
    public class HomeTheaterFacade(Computer computer, Projector projector, Screen screen)
    {
        private Computer computer = computer;
        private Projector projector = projector;
        private Screen screen = screen;

        public void WatchMovie()
        {
            Console.WriteLine("Esta lista para ver la pelicula...");
            computer.TurnOn();
            projector.TurnOn();
            screen.Down();
        }

        public void EndMovie()
        {
            Console.WriteLine("Se esta terminando la pelicula...");
            computer.TurnOff();
            projector.TurnOff();
            screen.Up();
        }
    }
}
