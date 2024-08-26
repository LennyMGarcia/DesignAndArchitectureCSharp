using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural
{
    // Abstract Component
    abstract class MenuComponent
    {
        public abstract void Display();
        public virtual void Add(MenuComponent component) { }
        public virtual void Remove(MenuComponent component) { }
        public virtual bool IsComposite() { return true; }
    }

    // Leaf: MenuItem
    // representa objetos simples que no tienen hijos
    class MenuItem(string name, string description, bool vegetarian, double price) : MenuComponent
    {
        private string _name = name;
        private string _description = description;
        private bool _vegetarian = vegetarian;
        private double _price = price;

        public override void Display()
        {
            Console.WriteLine($"  {(_vegetarian ? "(v)" : "")} {_name}");
            Console.WriteLine($"   -- {_description}");
            Console.WriteLine($"   -- {_price}");
        }

        public override bool IsComposite()
        {
            return false;
        }
    }

    // Composite: Menu
    //representa objetos complejos que pueden tener hijos
    class Menu(string name) : MenuComponent
    {
        private string _name = name;
        //permite anadir menu en el mismo menu, pero se usa mas para los menuItems
        private List<MenuComponent> _menuComponents = new List<MenuComponent>();

        public override void Display()
        {
            Console.WriteLine($"\n{_name}");
            Console.WriteLine("---------------------");

            foreach (var component in _menuComponents)
            {
                component.Display();
            }
        }

        public override void Add(MenuComponent component)
        {
            _menuComponents.Add(component);
        }

        public override void Remove(MenuComponent component)
        {
            _menuComponents.Remove(component);
        }
    }
}
