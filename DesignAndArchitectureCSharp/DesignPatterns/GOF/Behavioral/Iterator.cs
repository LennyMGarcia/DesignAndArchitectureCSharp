using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    // Iterator interface
    public interface IBookshelfIterator : IEnumerator
    {
        new object Current { get; }
        //El new hace que cree una nueva implementacion y oculte la heredada
        new bool MoveNext();
        new void Reset();
    }

    // Concrete iterator
    //Mayormente sirve para estructuras de datos complejas en donde quieres iteralas de la forma que quieres
    // Podrias usar un for...in facilmente sin tener que crear ciclos complejos para recorrer toda la estructura
    public class LibraryBookshelfIterator(List<Book> books) : IBookshelfIterator
    {
        private readonly List<Book> _books = books;
        private int _position = -1;

        public object Current => _books[_position];

        public bool MoveNext()
        {
            _position++;
            return _position < _books.Count;
        }

        public void Reset()
        {
            _position = -1;
        }
    }

    // Aggregate interface
    public interface IBookshelf : IEnumerable
    {
        new IEnumerator GetEnumerator();
    }

    // Concrete aggregate
    public class LibraryBookshelf : IBookshelf
    {
        private readonly List<Book> _books = new List<Book>();

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        public IEnumerator GetEnumerator()
        {
            return new LibraryBookshelfIterator(_books);
        }
    }
    public class Book(string title)
    {
        public string Title { get; set; } = title;
    }
}
