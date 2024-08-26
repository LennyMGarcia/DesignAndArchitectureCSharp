using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    public interface IHtmlElement
    {
        void Accept(IHtmlElementVisitor visitor);
    }

    // Concrete html elements
    public class HtmlBody : IHtmlElement
    {
        public void Accept(IHtmlElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetBodyContent()
        {
            return "Body content";
        }
    }

    public class HtmlHead : IHtmlElement
    {
        public void Accept(IHtmlElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetHeadTitle()
        {
            return "Head title";
        }
    }

    public class HtmlDiv : IHtmlElement
    {
        public void Accept(IHtmlElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetDivContent()
        {
            return "Div content";
        }
    }

    // Visitor interface
    public interface IHtmlElementVisitor
    {
        void Visit(HtmlBody body);
        void Visit(HtmlHead head);
        void Visit(HtmlDiv div);
    }

    // Concrete visitor
    public class HtmlElementPrinter : IHtmlElementVisitor
    {
        public void Visit(HtmlBody body)
        {
            Console.WriteLine("Visitando HtmlBody");
            Console.WriteLine("Body content: " + body.GetBodyContent());
        }

        public void Visit(HtmlHead head)
        {
            Console.WriteLine("Visitando HtmlHead");
            Console.WriteLine("Head title: " + head.GetHeadTitle());
        }

        public void Visit(HtmlDiv div)
        {
            Console.WriteLine("Visitando HtmlDiv");
            Console.WriteLine("Div content: " + div.GetDivContent());
        }
    }

    public class HtmlDocument
    {
        private List<IHtmlElement> elements = new List<IHtmlElement>();

        public void AddElement(IHtmlElement element)
        {
            elements.Add(element);
        }

        public void Accept(IHtmlElementVisitor visitor)
        {
            //La logica principal esta aqui, cuando se accede a los elementos una ves anadidos se pasa la clase visitadora como visitor
            //Si es div por ejemplo, div mandara un visit(this) y en la clase visitadora se buscara una sobrecarga que coincida con el objeto this
            // asi se permite agregar nuevas operaciones a una estructura de objetos sin modificar la estructura misma, o modificandola lijeramente
            //permite separar la logica de negocio de la estructura de objetos, lo que hace que sea más facil de mantener y extender la aplicacion
            foreach (IHtmlElement element in elements)
            {
                element.Accept(visitor);
            }
        }
    }
}
