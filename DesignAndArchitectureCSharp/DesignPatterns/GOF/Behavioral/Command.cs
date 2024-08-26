using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // Concrete Commands
    // los objetos Command encapsulan una solicitud o accióon especifica que se puede realizar en un objeto Receiver 
    public class LightOnCommand(Light light) : ICommand
    {
        public readonly Light _light = light;

        public void Execute()
        {
            _light.SwitchOn();
        }

        public void Undo()
        {
            _light.SwitchOff();
        }
    }

    public class LightOffCommand(Light light) : ICommand
    {
        public readonly Light _light = light;

        public void Execute()
        {
            _light.SwitchOff();
        }

        public void Undo()
        {
            _light.SwitchOn();
        }
    }

    // Receiver
    //Los receiver imparten las acciones
    public class Light
    {
        public void SwitchOn()
        {
            Console.WriteLine("La luz esta prendida");
        }

        public void SwitchOff()
        {
            Console.WriteLine("La luz esta apagada");
        }
    }

    // Invoker
    public class RemoteControl
    {
        private ICommand? _command;
        //El RemoteControl basicamente puede tener cualquier comando y solo tendra que usar el Execute
        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void PressButton()
        {
            if (_command != null)
            {
                _command.Execute();
            }
        }
    }

    // CommandHistory
    // Guardara el historial de comandos, asi si quieres dar vuelta atras solo ejecutas el undo
    public class CommandHistory
    {
        private readonly List<ICommand> _commands = new List<ICommand>();
        private int _currentIndex = -1;

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
            _currentIndex++;
        }

        public void Undo()
        {
            if (_currentIndex >= 0)
            {
                //usa el undo del comando
                ICommand command = _commands[_currentIndex];
                command.Undo();
                _currentIndex--;
            }
        }

        public void Redo()
        {
            if (_currentIndex + 1 < _commands.Count)
            {
                _currentIndex++;
                ICommand command = _commands[_currentIndex];
                command.Execute();
            }
        }
    }
}
