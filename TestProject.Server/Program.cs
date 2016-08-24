using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Threading.Timer;

namespace TestProject.Server
{
    class Program
    {
        private static Timer _timer;
        static void Main(string[] args)
        {
            Settings.SaveSettings();
            try
            {
                _timer = new Timer(_ => SendMessage(), null, 1000, Timeout.Infinite);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong ...");
            }
        }


        public static void SendMessage()
        {
            var random = new Random();
            //while (true)
            //{
            var settings = Settings.GetSettings();
            var msg = random.Next(settings.LowerBoundary, settings.UpperBoundary);

            var data = Encoding.Default.GetBytes(msg.ToString());
            using (var udpClient = new UdpClient(AddressFamily.InterNetwork))
            {
                var address = IPAddress.Parse(Settings.GetSettings().IpAddress);
                var ipEndPoint = new IPEndPoint(address, Settings.GetSettings().Port);
                udpClient.JoinMulticastGroup(address);
                udpClient.Send(data, data.Length, ipEndPoint);
                udpClient.Close();
            }
            _timer.Change(1000, Timeout.Infinite);
            Console.WriteLine(msg);
            //}
        }
    }
}