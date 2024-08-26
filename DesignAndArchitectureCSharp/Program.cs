using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignAndArchitectureCSharp.DesignPatterns.GOF;
using DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral;
using DesignAndArchitectureCSharp.DesignPatterns.GOF.Creational;
using DesignAndArchitectureCSharp.DesignPatterns.GOF.Structural;
using DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceRelease;
using DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceAcquisition;
using DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA3.ResourceLifecycle;
using DesignAndArchitectureCSharp.DesignPatterns.POSA.POSA2.Concurrency;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("\n -------CREATIONAL");
            Console.WriteLine("SINGLETON");

            Singleton_SystemConfiguration config1 = Singleton_SystemConfiguration.Instance;
            Singleton_SystemConfiguration config2 = Singleton_SystemConfiguration.Instance;
            Singleton_SystemConfiguration config3 = Singleton_SystemConfiguration.Instance;

            Console.WriteLine("son config1 y config2 lo mismo? " + (config1 == config2));
            Console.WriteLine("son config2 y config3 lo mismo? " + (config2 == config3));
            Console.WriteLine("son config1 y config3 lo mismo? " + (config1 == config3));

            config1.Initialize("new-server");

            Console.WriteLine("config1.DatabaseServer: " + config1.DatabaseServer);
            Console.WriteLine("config2.DatabaseServer: " + config2.DatabaseServer);
            Console.WriteLine("config3.DatabaseServer: " + config3.DatabaseServer);

            Console.WriteLine("\n FACTORY");

            IPayment payment = PaymentFactory.CreatePayment("creditcard", 100.00m);
            payment.MakePayment(100.00m);

            payment = PaymentFactory.CreatePayment("paypal", 50.00m);
            payment.MakePayment(50.00m);

            payment = PaymentFactory.CreatePayment("banktransfer", 200.00m);
            payment.MakePayment(200.00m);

            Console.WriteLine("\n ABSTRACT FACTORY");

            // Crear una fabrica de gerentes y departamentos de recursos humanos
            EmployeeFactory factory = new ManagementFactory();

            // Crear un gerente
            Employee manager = factory.CreateEmployee("John Doe", 35);
            Console.WriteLine($"Empleado: {manager.Name}, Edad: {manager.Age}");

            // Crear un departamento de recursos humanos
            Department department = factory.CreateDepartment("Recursos Humanos");
            Console.WriteLine($"Departamento: {department.Name}");

            // Crear una fabrica de desarrolladores y departamentos de tecnologia
            factory = new DevelopmentFactory();

            // Crear un desarrollador
            Employee developer = factory.CreateEmployee("Jane Doe", 28);
            Console.WriteLine($"Empleado: {developer.Name}, Edad: {developer.Age}");

            // Crear un departamento de tecnologia
            department = factory.CreateDepartment("Tecnología");
            Console.WriteLine($"Departamento: {department.Name}");

            Console.WriteLine("\n BUILDER");

            OrderBuilder builder = new OrderBuilder();
            OrderDirector director = new OrderDirector(builder);
            var keyboardOrder = director.CreateKeyboardOrder("Lenny", "123 ft");
            Console.WriteLine($"Pedido de {keyboardOrder.CustomerName}: con {keyboardOrder.Quantity} unidades de {keyboardOrder.ProductName} a ${keyboardOrder.Price} para ser entegado en {keyboardOrder.ShippingAddress}");

            var computerOrder = director.CreateComputerOrder("Elen", "125 ft");
            Console.WriteLine($"Pedido de {computerOrder.CustomerName}: con {computerOrder.Quantity} unidades de {computerOrder.ProductName} a ${computerOrder.Price} para ser entegado en {computerOrder.ShippingAddress}");

            Console.WriteLine("\n PROTOTYPE");
            var originalPersona = new Persona { Nombre = "Juan Pérez", Edad = 30, Salario = 50000.00m, Direccion = "Calle 123" };
            var clonedPersona = originalPersona.Clone();

            Console.WriteLine("Persona original:");
            Console.WriteLine($"Nombre: {originalPersona.Nombre}, Edad: {originalPersona.Edad}, Salario: {originalPersona.Salario}, Direccion: {originalPersona.Direccion}");

            Console.WriteLine("Persona clonada:");
            Console.WriteLine($"Nombre: {clonedPersona.Nombre}, Edad: {clonedPersona.Edad}, Salario: {clonedPersona.Salario}, Direccion: {clonedPersona.Direccion}");

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("\n -------ESTRUCTURAL");
            Console.WriteLine("\n ADAPTER");
            var tty2Writer = new TTY2Writer();
            var loggerAdapter = new LoggerAdapter(tty2Writer);
            loggerAdapter.WriteLog("Este es un registro de prueba");

            Console.WriteLine("\n COMPOSITE");
            MenuComponent menuDeLaCasaDePancakes = new Menu("Menu de la Casa de Pancakes");
            MenuComponent menuDelDiner = new Menu("Menu del Diner");
            MenuComponent menuDelCafé = new Menu("Menu del Café");

            MenuComponent desayunoItems = new Menu("Desayuno");
            MenuComponent almuerzoItems = new Menu("Almuerzo");
            MenuComponent cenaItems = new Menu("Cena");

            MenuItem pancakes = new MenuItem("Desayuno de Pancakes K&B", "Pancakes con huevos revueltos y tostada", true, 2.99);
            MenuItem dinerDesayuno = new MenuItem("Desayuno Modelo Regular", "Pancakes, huevos revueltos, salchicha", false, 3.99);
            MenuItem blt = new MenuItem("BLT", "Tocino, lechuga y tomate en pan integral", false, 4.99);

            desayunoItems.Add(pancakes);
            almuerzoItems.Add(dinerDesayuno);
            almuerzoItems.Add(blt);

            menuDeLaCasaDePancakes.Add(desayunoItems);
            menuDelDiner.Add(almuerzoItems);
            menuDelDiner.Add(cenaItems);

            menuDelCafé.Add(new MenuItem("Hamburguesa de Vegetales y Papas Fritas", "Hamburguesa de vegetales en un bollo de pan integral con papas fritas", true, 3.99));
            menuDelCafé.Add(new MenuItem("Sopa del Dia", "Una taza de la sopa del dia, con una ensalada lateral", false, 3.69));

            MenuComponent todosLosMenus = new Menu("Todos los Menus");
            todosLosMenus.Add(menuDeLaCasaDePancakes);
            todosLosMenus.Add(menuDelDiner);
            todosLosMenus.Add(menuDelCafé);

            todosLosMenus.Display();

            Console.WriteLine("\n PROXY");

            Client client = new Client();

            Console.WriteLine("Client: Ejecuntando el codigo del cliente con los datos de acceso reales:");
            RealDataAccess realDataAccess = new RealDataAccess();
            client.ClientCode(realDataAccess);

            Console.WriteLine();

            Console.WriteLine("Client: Ejecuntando el codigo del cliente con los datos de acceso del proxy:");
            DataAccessProxy proxy = new DataAccessProxy(realDataAccess);
            client.ClientCode(proxy);

            Console.WriteLine("\n FLYWEIGHT");
            var textEditor = new TextEditor();

            textEditor.AddCharacter('A', "Arial", 1, 12);
            textEditor.AddCharacter('B', "Times New Roman", 2, 14);
            textEditor.AddCharacter('A', "Arial", 1, 12);

            Console.WriteLine("Displaying all characters:");
            textEditor.DisplayAllCharacters();

            Console.WriteLine("\n FACADE");

            Computer computer = new Computer();
            Projector projector = new Projector();
            Screen screen = new Screen();

            HomeTheaterFacade homeTheater = new HomeTheaterFacade(computer, projector, screen);

            homeTheater.WatchMovie();
            homeTheater.EndMovie();

            Console.WriteLine("\n BRIDGE");
            IEngine gasolineEngine = new GasolineEngine();
            IEngine electricEngine = new ElectricEngine();

            Vehicle carGasoline = new Car(gasolineEngine);
            Vehicle carElectric = new Car(electricEngine);

            Vehicle truckGasoline = new Truck(gasolineEngine);
            Vehicle truckElectric = new Truck(electricEngine);

            Console.WriteLine(carGasoline.StartEngine());
            Console.WriteLine(carGasoline.Accelerate());
            Console.WriteLine(carElectric.StartEngine());
            Console.WriteLine(carElectric.Accelerate());
            Console.WriteLine(truckGasoline.StartEngine());
            Console.WriteLine(truckGasoline.Accelerate());
            Console.WriteLine(truckElectric.StartEngine());
            Console.WriteLine(truckElectric.Accelerate());

            Console.WriteLine("\n DECORATOR");
            CoffeeComponent coffee = new SimpleCoffee();
            Console.WriteLine($"Descripcion: {coffee.Description()}, Costo: ${coffee.Cost()}");

            coffee = new MochaDecorator(coffee);
            Console.WriteLine($"Descripcion: {coffee.Description()}, Costo: ${coffee.Cost()}");

            coffee = new WhippedCreamDecorator(coffee);
            Console.WriteLine($"Descripcion: {coffee.Description()}, Costo: ${coffee.Cost()}");

            //-----------------------------------------------------------------------------------------
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("\n -------BEHAVORIAL");
            Console.WriteLine("\n TEMPLATE METHOD");
            DocumentProcessor processor = new TextDocumentProcessor();
            processor.ProcessDocument();
            processor = new ImageDocumentProcessor();
            processor.ProcessDocument();

            Console.WriteLine("\n MEDIATOR");
            IMediator mediator = new ChatMediator();

            User user1 = new User("Lenny");
            User user2 = new User("Elen");
            User user3 = new User("Carlos");

            //Se pone quien sera el mediador
            user1.SetMediator(mediator);
            user2.SetMediator(mediator);
            user3.SetMediator(mediator);

            //El mediador recive los usuarios que se manejaran
            mediator.RegisterColleague(user1);
            mediator.RegisterColleague(user2);
            mediator.RegisterColleague(user3);

            //los otros usuarios deben recibirlo
            user1.UploadFile("example.txt");
            user1.SendMessage("hola");

            Console.WriteLine("\n CHAIN OF RESPONSIBILITY");
            ITransactionHandler lowValueHandler = new LowValueTransactionHandler();
            ITransactionHandler mediumValueHandler = new MediumValueTransactionHandler();
            ITransactionHandler highValueHandler = new HighValueTransactionHandler();

            //Aqui se pone la cadena
            lowValueHandler.NextHandler = mediumValueHandler;
            mediumValueHandler.NextHandler = highValueHandler;

            for (int i = 0; i < 20000; i += 1000)
            {
                COR_Payment cor_payment = new COR_Payment { Amount = i };
                lowValueHandler.HandleTransaction(cor_payment);
            }

            Console.WriteLine("\n OBSERVER");

            IWeatherStation weatherStation = new WeatherStation();

            var forecastDisplay = new ForecastDisplay(weatherStation);

            weatherStation.NotifyObservers(new WeatherData
            {
                Temperature = 25.0f,
                Humidity = 60.0f,
                Pressure = 1013.25f
            });

            Console.WriteLine("\n STRATEGY");
            ICompressionAlgorithm zipAlgorithm = new ZipCompression();
            ICompressionAlgorithm rarAlgorithm = new RarCompression();

            DataCompressor compressor = new DataCompressor(zipAlgorithm);
            compressor.CompressData("Hello, World!");

            compressor.SetAlgorithm(rarAlgorithm);
            compressor.CompressData("Hello, World!");

            Console.WriteLine("\n COMMAND");
            //Reciver es light que le impartira acciones a los commands
            Light light = new Light();
            //Los commands guardaran acciones especificas asociadas a su funcionalidad
            ICommand lightOnCommand = new LightOnCommand(light);
            ICommand lightOffCommand = new LightOffCommand(light);

            RemoteControl remoteControl = new RemoteControl();
            CommandHistory commandHistory = new CommandHistory();

            Console.WriteLine("Presionando boton on...");
            remoteControl.SetCommand(lightOnCommand);
            remoteControl.PressButton();
            commandHistory.AddCommand(lightOnCommand);

            Console.WriteLine("Presionando boton off...");
            remoteControl.SetCommand(lightOffCommand);
            remoteControl.PressButton();
            commandHistory.AddCommand(lightOffCommand);

            Console.WriteLine("Presionando boton undo...");
            commandHistory.Undo();

            Console.WriteLine("Presionando boton redo...");
            commandHistory.Redo();

            Console.WriteLine("\n STATE");

            //Comenzamos con la luz roja, cada request debe cambiar el estado
            TrafficLightContext trafficLight = new TrafficLightContext(new RedLightState());

            trafficLight.Request();
            trafficLight.Request();
            trafficLight.Request();

            Console.WriteLine("\n VISITOR");
            HtmlDocument document = new HtmlDocument();
            document.AddElement(new HtmlBody());
            document.AddElement(new HtmlHead());
            document.AddElement(new HtmlDiv());

            IHtmlElementVisitor printer = new HtmlElementPrinter();
            document.Accept(printer);

            Console.WriteLine("\n ITERATOR");

            LibraryBookshelf bookshelf = new LibraryBookshelf();
            bookshelf.AddBook(new Book("Libro de Tarzan"));
            bookshelf.AddBook(new Book("Libro de la bella y la betia"));
            bookshelf.AddBook(new Book("Libro de caperucita roja"));

            foreach (Book book in bookshelf)
            {
                Console.WriteLine(book.Title);
            }

            Console.WriteLine("\n MEMENTO");

            Game game = new Game(3, 0);
            GameHistory history = new GameHistory();

            game.SetLives(2);
            history.Save(game.Save());

            game.SetScore(100);
            history.Save(game.Save());

            game.SetLives(1);
            history.Save(game.Save());

            game.Restore(history.Restore(1));
            Console.WriteLine($"Vidas: {game._lives}, Puntuacion: {game._score}");


            Console.WriteLine("\n INTERPRETER");
            MathInterpreter interpreter = new MathInterpreter();

            string expression = "2 + 3 * 4";
            double result = interpreter.Evaluate(expression);
            Console.WriteLine($"Resultado: {result}");

            expression = "10 - 2 + 5";
            result = interpreter.Evaluate(expression);
            Console.WriteLine($"Resultado: {result}");

            expression = "8 / 2 - 1";
            result = interpreter.Evaluate(expression);
            Console.WriteLine($"Resultado: {result}");

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("\n POSA");
            Console.WriteLine("POSA3");
            Console.WriteLine("RESOURCE RELEASE");

            Console.WriteLine("\n EVICTOR");

            var connectionPool = new ConnectionPool();

            var connection1 = new DatabaseConnection("Conexion 1");
            var connection2 = new DatabaseConnection("Conexion 2");
            var connection3 = new DatabaseConnection("Conexion 3");

            connectionPool.Add(connection1);
            connectionPool.Add(connection2);
            connectionPool.Add(connection3);

            var evictor = new ConnectionEvictor(connectionPool, TimeSpan.FromSeconds(5));

            connection1.ExecuteQuery("SELECT * FROM tabla1");
            connection2.ExecuteQuery("SELECT * FROM tabla2");
            connection3.ExecuteQuery("SELECT * FROM tabla3");

            Thread.Sleep(TimeSpan.FromSeconds(10));

            Console.WriteLine("Conexiones activas:");
            foreach (var connection in connectionPool)
            {
                Console.WriteLine(connection.ConnectionString);
            }

            Console.WriteLine("\n LEASE");

            var pool = new CachePool(() => new MemoryCache(), c => c.Dispose());

            CacheLease l;
            using (var lease = pool.Acquire())
            {

                lease.Cache.Add("clave", "valor");
                Console.WriteLine("Recurso liberado: " + (lease.IsValid ? "No" : "Si"));
                Console.WriteLine(lease.Cache.Get("clave"));
                l = (CacheLease)lease;
            }

            // se verifica que el recurso se ha liberado
            Console.WriteLine("Recurso liberado: " + (l.IsValid ? "No" : "Si"));

            Console.WriteLine("\n RESOURCE ACQUISITION");

            Console.WriteLine("\n LOOKUP");

            var lookup = new WorkspaceSessionLookup(sessionId => new WorkspaceSession { SessionId = sessionId });

            using (var session1 = lookup.GetSession("Session1"))
            {
                session1.Start();
                Console.WriteLine("Usando seccion 1...");
            }

            using (var session2 = lookup.GetSession("Session2"))
            {
                session2.Start();
                Console.WriteLine("Usando seccion 2...");
            }

            using (var session3 = lookup.GetSession("Session1"))
            {
                session3.Start();
                Console.WriteLine("Usando seccion 1 de nuevo...");
            }

            Console.WriteLine("\n LAZY ACQUISITION");

            LazyAcquisition<ExpensiveResource> lazyResource = new LazyAcquisition<ExpensiveResource>();

            Console.WriteLine("Antes de obtener el recurso...");

            ExpensiveResource recurso = lazyResource.Get();
            recurso.DoSomething();

            // No se vuelve a adquirir el recurso
            Console.WriteLine("Despues de obtener el recurso...");
            recurso.DoSomething();

            Console.WriteLine("\n EAGER ACQUISITION");

            var eagerAcquisition = new EagerAcquisition<ExpensiveResource>();

            var resource = eagerAcquisition.Get();
            resource.DoSomething();

            eagerAcquisition.Dispose();

            Console.WriteLine("\n RESOURCE LIFECYCLE MANAGER");

            var rlm = new ResourceLifecycleManager<YourResource>(() => new YourResource(), resource => resource.Initialize(), resource => resource.Finalize(), 5);

            var resource2 = rlm.AcquireResource();
            var resource3 = rlm.AcquireResource();

            rlm.ReleaseResource(resource2);
            rlm.ReleaseResource(resource3);
            rlm.CleanUp();

            Console.WriteLine("\n Coordinator");

            var coordinator = new ResourceLifecycleCoordinator();

            string printerId = "mi_impresora";
            var printerFactory = new PrinterFactory();
            var printer2 = coordinator.Acquire(printerId, printerFactory.Create);
            Console.WriteLine($"Adquirida impresora: {printerId}");

            printer2.Print("Hola, mundo!");
            Console.WriteLine("Utilizando la impresora...");

            coordinator.Release(printerId);
            Console.WriteLine("Liberada impresora");

            try
            {
                coordinator.Acquire(printerId, printerFactory.Create);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message); 
            }

            try
            {
                coordinator.Get<Printer>(printerId);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message); 
            }

            coordinator.Release(printerId);

            var printer3 = coordinator.Acquire(printerId, printerFactory.Create);
            Console.WriteLine($"Adquirida impresora de nuevo: {printerId}");

            Console.WriteLine("\n POOLING");

            var resourcePool = new Posa3ResourcePool<MiRecurso>(5, 1, TimeSpan.FromHours(60));

            var r = resourcePool.Checkout();
            var mr = new MiRecurso();
            resourcePool.Add(mr);

            r.Initialize();
            Console.WriteLine("Recurso inicializado");

            resourcePool.Checkin(r);

            var anotherResource = resourcePool.Checkout();

            anotherResource.Initialize();
            Console.WriteLine("Otro recurso inicializado");

            // Liberar todos los recursos del pool
            //resourcePool.ReleaseAllResources();
            //resourcePool.Evict();

            Console.WriteLine("\n CACHING");


            var rl_cache = new RL_ResourceCache<string, string>();
            Func<string> rl_createResource = () => "Hello, World!";
            string rl_resource = rl_cache.GetResource("myResource", rl_createResource);

            Console.WriteLine("Recurso obtenido: " + rl_resource);

            //rl_cache.ReleaseResource("myResource");

            try
            {
                rl_resource = rl_cache.GetResource("myResource", rl_createResource);
                Console.WriteLine("Recurso obtenido de nuevo: " + rl_resource);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            rl_cache.InvalidateResource("myResource");

            try
            {
                rl_resource = rl_cache.GetResource("myResource", rl_createResource);
                Console.WriteLine("Recurso obtenido de nuevo: " + rl_resource);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Erroooor: " + ex.Message);
            }

            Console.WriteLine("POSA2");
            Console.WriteLine("\n CONCURRENCY");

            Console.WriteLine("\n ACTIVE OBJECT");

            /* P2_MessageQueue messageQueue = new P2_MessageQueueImpl();
             P2_ActiveObject activeObject = new P2_ActiveObjectImpl(messageQueue);
             activeObject.Start();

             // Enviar mensajes al objeto activo
             activeObject.Enqueue(new P2_OrderMessage("Pasta"));
             activeObject.Enqueue(new P2_OrderMessage("Burger"));
             activeObject.Enqueue(new P2_OrderMessage("Salad"));


             activeObject.WaitForCompletion();
             activeObject.Stop(); */

            Console.WriteLine("\n MONITOR OBJECT");

            ResourceGuard resourceGuard = new ResourceGuard();

            // Test EnterResource y ExitResource
            TestEnterExit(resourceGuard);

            // Test WaitForResource y SignalResource
            TestWaitSignal(resourceGuard);

            // Test SignalAllResources
            TestSignalAll(resourceGuard);

            // Test TryEnterResource con timeout
            TestTryEnterTimeout(resourceGuard);

            // Test TryEnterResource con TimeSpan
            TestTryEnterTimeSpan(resourceGuard);

            Console.WriteLine("\n THREAD SPECIFIC STORAGE");


            P2_ThreadSpecificStorageFactory Tssfactory = new P2_ThreadSpecificStorageFactory();

            P2_ThreadSpecificStorage<int> intStorage = Tssfactory.CreateStorage<int>();
            intStorage.SetValue(10);
            Console.WriteLine("Hilo 1: Valor = " + intStorage.GetValue());

            P2_ThreadSpecificStorage<string> stringStorage = Tssfactory.CreateStorage<string>();
            stringStorage.SetValue("Hello, World!");
            Console.WriteLine("Hilo 1: Valor = " + stringStorage.GetValue());

            Thread thread2 = new Thread(() =>
            {
                P2_ThreadSpecificStorage<int> intStorage2 = Tssfactory.CreateStorage<int>();
                intStorage2.SetValue(20);
                Console.WriteLine("Hilo 2: Valor = " + intStorage2.GetValue());
            });
            thread2.Start();
            thread2.Join();

            Console.WriteLine("Hilo 1: Valor = " + intStorage.GetValue());


            Console.WriteLine("\n LEADER FOLLOWERS");

            LeaderFollower leaderFollower = new LeaderFollower(5); 

            leaderFollower.Start(); 

            // Simular eventos
            for (int i = 0; i < 10; i++)
            {
                leaderFollower.SignalEvent($"Evento {i}");
                Thread.Sleep(100); 
            }

            leaderFollower.Stop(); 
            Console.WriteLine("\n HALF-SYNC HALF-ASYNC");

            var concurrencyGate = new P2_ConcurrencyGate(5);
            var tasks = new List<Task>();

            for (int i = 1; i <= 10; i++)
            {
                tasks.Add(concurrencyGate.ProcessRequestAsync(i));
            }

            await Task.WhenAll(tasks);




        }

        static void TestEnterExit(ResourceGuard resourceGuard)
        {
            Console.WriteLine("Testing EnterResource y ExitResource");

            Thread thread1 = new Thread(() =>
            {
                resourceGuard.EnterResource();
                Console.WriteLine("Thread 1 entro al recurso");
                Thread.Sleep(1000);
                resourceGuard.ExitResource();
                Console.WriteLine("Thread 1 salio del recurso");
            });

            Thread thread2 = new Thread(() =>
            {
                resourceGuard.EnterResource();
                Console.WriteLine("Thread 2 entro al recurso");
                Thread.Sleep(1000);
                resourceGuard.ExitResource();
                Console.WriteLine("Thread 2 salio del recurso");
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        static void TestWaitSignal(ResourceGuard resourceGuard)
        {
            Console.WriteLine("Testing WaitForResource y SignalResource");

            Thread thread1 = new Thread(() =>
            {
                resourceGuard.WaitForResource();
                Console.WriteLine("Thread 1 consiguo el recurso");
                Thread.Sleep(1000);
            });

            Thread thread2 = new Thread(() =>
            {
                Thread.Sleep(500);
                resourceGuard.SignalResource();
                Console.WriteLine("Thread 2 senalo el recurso");
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        static void TestSignalAll(ResourceGuard resourceGuard)
        {
            Console.WriteLine("Testing SignalAllResources");

            Thread thread1 = new Thread(() =>
            {
                resourceGuard.WaitForResource();
                Console.WriteLine("Thread 1 consiguo el recurso");
                Thread.Sleep(1000);
            });

            Thread thread2 = new Thread(() =>
            {
                resourceGuard.WaitForResource();
                Console.WriteLine("Thread 2 consiguo el recurso");
                Thread.Sleep(1000);
            });

            Thread thread3 = new Thread(() =>
            {
                Thread.Sleep(500);
                resourceGuard.SignalAllResources();
                Console.WriteLine("Thread 3 aviso a todos los recurso");
            });

            thread1.Start();
            thread2.Start();
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();
        }

        static void TestTryEnterTimeout(ResourceGuard resourceGuard)
        {
            Console.WriteLine("Testing TryEnterResource con timeout");

            Thread thread1 = new Thread(() =>
            {
                if (resourceGuard.TryEnterResource(1000))
                {
                    Console.WriteLine("Thread 1 entro al recurso");
                    Thread.Sleep(1000);
                    resourceGuard.ExitResource();
                    Console.WriteLine("Thread 1 salio del recurso");
                }
                else
                {
                    Console.WriteLine("Thread 1 timed out");
                }
            });

            Thread thread2 = new Thread(() =>
            {
                resourceGuard.EnterResource();
                Console.WriteLine("Thread 2 entro al recurso");
                Thread.Sleep(2000);
                resourceGuard.ExitResource();
                Console.WriteLine("Thread 2 salio del recurso");
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        static void TestTryEnterTimeSpan(ResourceGuard resourceGuard)
        {
            Console.WriteLine("Testing TryEnterResource conTimeSpan");

            Thread thread1 = new Thread(() =>
            {
                if (resourceGuard.TryEnterResource(TimeSpan.FromMilliseconds(1000)))
                {
                    Console.WriteLine("Thread 1 entro al recurso");
                    Thread.Sleep(1000);
                    resourceGuard.ExitResource();
                    Console.WriteLine("Thread 1 salio del recurso");
                }
                else
                {
                    Console.WriteLine("Thread 1 se acabo su tiempo");
                }
            });

            Thread thread2 = new Thread(() =>
            {
                resourceGuard.EnterResource();
                Console.WriteLine("Thread 2 entro al recurso");
                Thread.Sleep(2000);
                resourceGuard.ExitResource();
                Console.WriteLine("Thread 2 salio del recurso");
            });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

        }

    }
}



