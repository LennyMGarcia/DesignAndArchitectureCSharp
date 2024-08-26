using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Creational
{
    public interface IPayment
    {
        void MakePayment(decimal amount);
    }

    // Clase abstracta para los objetos de pago
    // Util para heredar la inicializacion
    public abstract class Payment(decimal amount) : IPayment
    {
        protected decimal _amount = amount;

        public abstract void MakePayment(decimal amount);
    }

    // Clase concreta para tarjeta de credito
    public class CreditCardPayment(decimal amount) : Payment(amount)
    {
        public override void MakePayment(decimal amount)
        {
            Console.WriteLine($"Realizando pago con tarjeta de credito de ${amount}");
        }


    }

    // Clase concreta para PayPal
    public class PayPalPayment(decimal amount) : Payment(amount)
    {
        public override void MakePayment(decimal amount)
        {
            Console.WriteLine($"Realizando pago con PayPal de ${amount}");
        }
    }

    // Clase concreta para transferencia bancaria
    public class BankTransferPayment(decimal amount) : Payment(amount)
    {
        public override void MakePayment(decimal amount)
        {
            Console.WriteLine($"Realizando pago con transferencia bancaria de ${amount}");
        }
    }

    // Factory para crear objetos de pago
    public class PaymentFactory
    {

        public static IPayment CreatePayment(string paymentType, decimal amount)
        {
            switch (paymentType)
            {
                case "creditcard":
                    return new CreditCardPayment(amount);
                case "paypal":
                    return new PayPalPayment(amount);
                case "banktransfer":
                    return new BankTransferPayment(amount);
                default:
                    throw new ArgumentException("Tipo de pago no válido", nameof(paymentType));
            }
        }
    }
}
