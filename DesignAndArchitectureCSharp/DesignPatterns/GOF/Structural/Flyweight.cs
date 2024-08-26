using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural
{
    // Flyweight o patron de de objeto ligero
    public interface ICharacter
    {
        void Display(int fontCode, int fontSize);
    }

    public class CharacterFlyweight(char symbol, string fontFamily) : ICharacter
    {
        private readonly char _symbol = symbol;
        private readonly string _fontFamily = fontFamily;

        public void Display(int fontCode, int fontSize)
        {
            Console.WriteLine($"Mostrando caracter {_symbol} con codigo de fuente {fontCode} con tamano de fuente {fontSize} en {_fontFamily} como familia de fuente");
        }
    }

    // Flyweight Factory
    public class CharacterFactory
    {
        private readonly Dictionary<string, ICharacter> _characters = new Dictionary<string, ICharacter>();

        public ICharacter GetCharacter(char symbol, string fontFamily)
        {
            var key = $"{symbol}{fontFamily}";
            //si el valor existe entonces retorna el character del diccionario
            // he aqui el flyweight
            if (_characters.TryGetValue(key, out var character))
            {
                return character;
            }
            //sino, lo crea 
            var characterFlyweight = new CharacterFlyweight(symbol, fontFamily);
            _characters[key] = characterFlyweight;
            return characterFlyweight;
        }
    }

    public class TextEditor
    {
        private readonly List<ICharacter> _characters = new List<ICharacter>();
        private readonly CharacterFactory _characterFactory;

        public TextEditor()
        {
            _characterFactory = new CharacterFactory();
        }

        public void AddCharacter(char symbol, string fontFamily, int fontCode, int fontSize)
        {
            //se usa el flyweight y se guarda el character en la lista para retornarlo
            var character = _characterFactory.GetCharacter(symbol, fontFamily);
            _characters.Add(character);
            character.Display(fontCode, fontSize);
        }

        public void DisplayAllCharacters()
        {
            foreach (var character in _characters)
            {
                character.Display(1, 12);
            }
        }
    }
}
