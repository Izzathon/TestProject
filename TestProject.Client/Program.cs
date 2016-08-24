using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestProject.Client
{
    class Program
    {
        private static UdpClient _udpClient;
        private static List<int> _received;

        static void Main(string[] args)
        {
            var set = Settings.GetSettings();
            _udpClient = new UdpClient(set.Port);
            _udpClient.JoinMulticastGroup(IPAddress.Parse(set.IpAddress), 50);

            var receiveThread = new Thread(Receive);
            receiveThread.Start();
            Console.WriteLine("Press enter to see statistics.");
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("{0}\nPress Enter to see statistics!", Statistics());
                }
            }
        }

        public static void Receive()
        {
            _received = new List<int>();
            while (true)
            {
                var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = _udpClient.Receive(ref ipEndPoint);

                var message = Encoding.Default.GetString(data);
                _received.Add(int.Parse(message));
                //Console.WriteLine(Message);
            }
        }

        private static string Statistics()
        {
            string res = "Statistics are not available. Please try again later.";
            if (_received.Count != 0)
            {
                var avg = _received.Average();

                res = string.Format("Average: {0:N}\n St.Deviation: {1: N} \n Median: {2: N}", avg, GetStandardDeviation(), GetMedian());
                //TODO: add mode and package loss 
            }
            return res;
        }

        private static double GetStandardDeviation()
        {
            double m = 0.0;
            double s = 0.0;
            int k = 1;
            foreach (var value in _received)
            {
                double tmpM = m;
                m += (value - tmpM) / k;
                s += (value - tmpM) * (value - m);
                k++;
            }
            return Math.Sqrt(s / (k - 2));
        }

        private static double GetMedian()
        {
            var orderedList = _received.OrderBy(x => x).ToList();

            int listSize = orderedList.Count;
            double result;

            if (listSize % 2 == 0) // even
            {
                int midIndex = listSize / 2;
                result = (orderedList.ElementAt(midIndex - 1) + orderedList.ElementAt(midIndex)) / 2;
            }
            else // odd
            {
                double element = (double)listSize / 2;
                element = Math.Round(element, MidpointRounding.AwayFromZero);

                result = orderedList.ElementAt((int)(element - 1));
            }
            return result;
        }
    }
}