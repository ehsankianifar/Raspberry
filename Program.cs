using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.I2c.Drivers;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Iot.Device.DHTxx;
using Iot.Device.TMD2771;

namespace helloraspbian
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public static string myURL = "";
        static double temperature = 0;
        static double humidity = 0;

        static void Main(string[] args)
        {

            var pin = 17;
            var lightTimeInMilliseconds = 1000;
            var dimTimeInMilliseconds = 1000;

            var tempSensor = new DHTSensor(27, DhtType.Dht11);
            
            bool result = false;
            /*
            I2cConnectionSettings i2cSettings = new I2cConnectionSettings(1, 0x39);
            // get I2cDevice (in Linux)
            UnixI2cDevice i2cDevice = new UnixI2cDevice(i2cSettings);
            var lightSensor = new TMD2771(i2cDevice);
            lightSensor.init();
            */
            /////////

            /*
            void callback(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
            {
                Console.WriteLine("************Presed**************");
            }
            */
            /////////////


            GpioController controller = new GpioController();

            controller.OpenPin(22, PinMode.Input);
            //controller.RegisterCallbackForPinValueChangedEvent(22, PinEventTypes.Rising, callback);

            controller.OpenPin(pin, PinMode.Output);

            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs eventArgs) =>
            {
                controller.Dispose();
            };

            //Console.WriteLine("enter URL");
            //myURL = Console.ReadLine();
            //Console.WriteLine(myURL);
            myURL = "http://ehsankianifar-001-site1.itempurl.com/api/TempData";

            while (true)
            {
                controller.Write(pin, PinValue.High);
                Thread.Sleep(lightTimeInMilliseconds);
                controller.Write(pin, PinValue.Low);
                Thread.Sleep(dimTimeInMilliseconds);

                result = tempSensor.TryGetTemperatureAndHumidity(out temperature, out humidity);
                Console.WriteLine($"Temp in centigrad {temperature}");
                Console.WriteLine($"humidity in percent {humidity}");
                //Console.WriteLine($"result {result}");
                //Console.WriteLine($"light sensor c0 {lightSensor.ReadC0()}");
                //Console.WriteLine($"light sensor c1 {lightSensor.ReadC1()}");
                //Console.WriteLine($"light sensor id {lightSensor.ReadId()}");

                sendDataAsync();
                //TestAsync();
            }

        }
        private static async System.Threading.Tasks.Task<string> sendDataAsync()
        {
            string myJson = $"{{\"temperature\": \"{temperature}\",\"humidity\":\"{humidity}\"}}";
            var response = await client.PostAsync(
               myURL,
                new StringContent(myJson, Encoding.UTF8, "application/json"));
            string responseString = response.StatusCode.ToString();
            Console.WriteLine(responseString);
            return responseString;
        }

        private static async System.Threading.Tasks.Task<string> TestAsync()
        {
            var response = await client.GetAsync("http://httpbin.org/ip");
            string responseString = response.StatusCode.ToString();
            Console.WriteLine(responseString);
            return responseString;
        }
    }
}
