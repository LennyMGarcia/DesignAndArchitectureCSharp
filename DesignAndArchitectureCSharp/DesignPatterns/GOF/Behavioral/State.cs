using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    using System;

    // Traffic Light State Interface
    public interface ITrafficLightState
    {
        ITrafficLightState Handle();
        void PrintState();
    }

    // Red Light State
    public class RedLightState : ITrafficLightState
    {
        public ITrafficLightState Handle()
        {
            return new GreenLightState();
        }

        public void PrintState()
        {
            Console.WriteLine("La luz roja esta encendida.");
        }
    }

    // Green Light State
    public class GreenLightState : ITrafficLightState
    {
        public ITrafficLightState Handle()
        {
            return new YellowLightState();
        }

        public void PrintState()
        {
            Console.WriteLine("La luz verde esta escendida.");
        }
    }

    // Yellow Light State
    public class YellowLightState : ITrafficLightState
    {
        public ITrafficLightState Handle()
        {
            return new RedLightState();
        }

        public void PrintState()
        {
            Console.WriteLine("La luz amarilla esta encendida.");
        }
    }

    // Traffic Light Context
    public class TrafficLightContext(ITrafficLightState state)
    {
        private ITrafficLightState _state = state;

        public void Request()
        {
            _state.PrintState();
            // el estate pasa al otro y asi cambian de estados, es mejor hacerlos cicliclos,
            // si comenzamos con new RedLightState();, el nuevo state sera new GreenLightState(); y asi suscesivamente
            _state = _state.Handle();
        }
    }
}
