using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    //La idea es que si el objeto no puede manejar el problema, el problema pase a otro hasta que si pueda
    public class COR_Payment
    {
        public int Amount { get; set; }
    }

    public interface ITransactionHandler
    {
        void HandleTransaction(COR_Payment payment);
        ITransactionHandler NextHandler { get; set; }
    }

    public abstract class TransactionHandlerBase : ITransactionHandler
    {
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public ITransactionHandler? NextHandler { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

        //cada transaccion se pasara al otro por eso el nextHandler y la llamada de su metodo
        public virtual void HandleTransaction(COR_Payment payment)
        {
            if (NextHandler != null)
            {
                NextHandler.HandleTransaction(payment);
            }
            else
            {
                Console.WriteLine("La tansaccion no pudo ser procesada");
            }
        }
    }

    public class LowValueTransactionHandler : TransactionHandlerBase
    {
        public override void HandleTransaction(COR_Payment payment)
        {
            if (payment.Amount >= 0 && payment.Amount < 1000)
            {
                Console.WriteLine($"LowValueTransactionHandler: Procesando un pago de {payment.Amount} unidades");
            }
            else
            {
                base.HandleTransaction(payment);
            }
        }
    }

    public class MediumValueTransactionHandler : TransactionHandlerBase
    {
        public override void HandleTransaction(COR_Payment payment)
        {
            if (payment.Amount >= 1000 && payment.Amount < 10000)
            {
                Console.WriteLine($"MediumValueTransactionHandler: Procesando un pago de {payment.Amount} unidades");
            }
            else
            {
                base.HandleTransaction(payment);
            }
        }
    }

    public class HighValueTransactionHandler : TransactionHandlerBase
    {
        public override void HandleTransaction(COR_Payment payment)
        {
            if (payment.Amount >= 10000)
            {
                Console.WriteLine($"HighValueTransactionHandler: Procesando un pago de {payment.Amount} unidades");
            }
            else
            {
                base.HandleTransaction(payment);
            }
        }
    }
}
