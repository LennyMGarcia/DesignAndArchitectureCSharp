using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Memento permite a un objeto guardar y restaurar su estado interno sin violar la encapsulacion
namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    // Originator
    //  El objeto que tiene un estado interno que deseas guardar y restaurar
    public class Game(int lives, int score)
    {
        public int _lives = lives;
        public int _score = score;

        public void SetLives(int lives)
        {
            _lives = lives;
        }

        public void SetScore(int score)
        {
            _score = score;
        }
        //Crear un objeto que recuerda el estado actual
        //GameMemento es lo que se guardara en la lista
        public GameMemento Save()
        {
            return new GameMemento(_lives, _score);
        }
        //Recuerda el estado que le pases
        public void Restore(GameMemento memento)
        {
            //se accedera a ese objeto con los valores guardados y se usara
            _lives = memento.Lives;
            _score = memento.Score;
        }
    }

    // Memento
    // El objeto que representa el estado interno del Originato
    public class GameMemento(int lives, int score)
    {
        public int Lives { get; } = lives;
        public int Score { get; } = score;
    }

    // Caretaker
    // El objeto que se encarga de guardar y restaurar el Memento
    public class GameHistory
    {
        private List<GameMemento> _mementos = new List<GameMemento>();
        //Aqui se guardan las partidas
        public void Save(GameMemento memento)
        {
            _mementos.Add(memento);
        }
        //Aqui se recuerdan las partidas guardadas
        public GameMemento Restore(int index)
        {
            return _mementos[index];
        }
    }
}
