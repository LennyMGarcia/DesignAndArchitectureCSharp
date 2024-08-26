using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Creational
{
    // Producto que se hara
    public class Order
    {
        public string? CustomerName { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ShippingAddress { get; set; }
    }

    // Builder interface
    public interface IOrderBuilder
    {
        void SetCustomerName(string customerName);
        void SetProductName(string productName);
        void SetQuantity(int quantity);
        void SetPrice(decimal price);
        void SetShippingAddress(string shippingAddress);
        Order Build();
    }

    // implementacion del builder
    public class OrderBuilder : IOrderBuilder
    {
        private Order _order;

        public OrderBuilder()
        {
            _order = new Order();
        }

        public void SetCustomerName(string customerName)
        {
            _order.CustomerName = customerName;
        }

        public void SetProductName(string productName)
        {
            _order.ProductName = productName;
        }

        public void SetQuantity(int quantity)
        {
            _order.Quantity = quantity;
        }

        public void SetPrice(decimal price)
        {
            _order.Price = price;
        }

        public void SetShippingAddress(string shippingAddress)
        {
            _order.ShippingAddress = shippingAddress;
        }

        public Order Build()
        {
            return _order;
        }
    }

    // Director usa builder para crear un pedido
    public class OrderDirector(IOrderBuilder builder)
    {
        private IOrderBuilder _builder = builder;

        public Order CreateComputerOrder(string name, string address)
        {
            _builder.SetCustomerName(name);
            _builder.SetProductName("Computer");
            _builder.SetQuantity(1);
            _builder.SetPrice(199.99m);
            _builder.SetShippingAddress(address);
            return _builder.Build();
        }

        public Order CreateKeyboardOrder(string name, string address)
        {
            _builder.SetCustomerName(name);
            _builder.SetProductName("Keyboard");
            _builder.SetQuantity(1);
            _builder.SetPrice(18.99m);
            _builder.SetShippingAddress(address);
            return _builder.Build();
        }
    }

    
}
