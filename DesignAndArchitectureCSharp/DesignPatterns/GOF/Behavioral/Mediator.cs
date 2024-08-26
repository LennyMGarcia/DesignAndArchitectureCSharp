using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// se utiliza para reducir la complejidad de la comunicacion entre objetos en un sistema
namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    using System;
    using System.Collections.Generic;

    // Mediator interface
    public interface IMediator
    {
        void RegisterColleague(Colleague colleague);
        void Operation(object request, Colleague colleague);
    }

    // Concrete Mediator class
    // Mediator actua como un centro de comunicacion entre los objetos del sistema
    public class ChatMediator : IMediator
    {
        private List<Colleague> colleagues = new List<Colleague>();

        //Aqui se agregan los user a manejar
        public void RegisterColleague(Colleague colleague)
        {
            colleagues.Add(colleague);
        }

        public void Operation(object request, Colleague colleague)
        {
            //Si es un evento, entonces por cada colega se mandara el mensaje, solo si es diferente del user al que le pasamos como this
         
             if (request is string eventName)
            {
                foreach (var c in colleagues)
                {
                    if (c != colleague)
                    {
                        c.HandleEvent(eventName);
                    }
                }
            }

            else if (request is string message)
            {
                foreach (var c in colleagues)
                {
                    if (c != colleague)
                    {
                        c.ReceiveMessage(message);
                    }
                }
            }
        }
    }

    public interface Colleague
    {
        void SetMediator(IMediator mediator);
        void ReceiveMessage(string message);
        void HandleEvent(string eventName);
    }

    public class User(string name) : Colleague
    {
        private IMediator mediator;
        private string name = name;

        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public void SendMessage(string message)
        {
            Console.WriteLine($"{name} enviando: {message}");
            //this se refiere al User
            mediator.Operation("NewMessagePosted", this);
        }

        public void ReceiveMessage(string message)
        {
            Console.WriteLine($"{name} recibido: {message}");
        }

        public void UploadFile(string fileName)
        {
            Console.WriteLine($"{name} cargando archivo: {fileName}");
            mediator.Operation("NewFileUploaded", this);
        }

        public void HandleEvent(string eventName)
        {
            if (eventName == "NewFileUploaded")
            {
                UpdateFileList();
            }
            else if (eventName == "NewMessagePosted")
            {
                UpdateMessageList();
                NotifyNewMessage();
            }
        }

        private void UpdateFileList()
        {
            Console.WriteLine($"{name} actualizando lista de archivos.");
        }

        private void UpdateMessageList()
        {
            Console.WriteLine($"{name} actualizando lista de mensajes.");
        }

        private void NotifyNewMessage()
        {
            Console.WriteLine($"{name} Recivista una notificacion con un nuevo mensaje.");
        }
    }
}
