using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    public interface IWeatherStation
    {
        void RegisterObserver(IWeatherObserver observer);
        void RemoveObserver(IWeatherObserver observer);
        void NotifyObservers(WeatherData data);
    }

    public interface IWeatherObserver
    {
        void Update(WeatherData data);
    }

    public class WeatherStation : IWeatherStation
    {
        private List<IWeatherObserver> _observers = new List<IWeatherObserver>();

        public void RegisterObserver(IWeatherObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IWeatherObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(WeatherData data)
        {
            foreach (var observer in _observers)
            {
                //Notifica el cambio entre los observadores registrados en la estacion de clima
                observer.Update(data);
            }
        }
    }

    public class WeatherData
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
    }

    public class ForecastDisplay : IWeatherObserver
    {
        private IWeatherStation _weatherStation;
        private WeatherData _currentWeather;
        //Se pasa la estacion a observar y se anade este objeto ForecastDisplay
        //Cuando la estacion cambia un dato, se le notifica al forecast a traves del update
        // En ves de que Forecast tenga que Estar ligada a un obsevador, puedes elegir cualquier observador que desees evitando relaciones rigidas
        public ForecastDisplay(IWeatherStation weatherStation)
        {
            _weatherStation = weatherStation;
            _weatherStation.RegisterObserver(this);
        }

        public void Update(WeatherData data)
        {
            //Se pasan los datos y se representan
            _currentWeather = data;
            DisplayForecast();
        }

        private void DisplayForecast()
        {
            Console.WriteLine("Prevision: ");
            Console.WriteLine($"Temperatura: {_currentWeather.Temperature}°C");
            Console.WriteLine($"Humedad: {_currentWeather.Humidity}%");
            Console.WriteLine($"Presion: {_currentWeather.Pressure} hPa");
        }
    }
}
