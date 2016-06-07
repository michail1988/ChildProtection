using System;
using System.Net;
using System.Net.NetworkInformation;

namespace ChildProtectionConsoleTests
{
    public class NetworkingExample
    {
        public static void Execute()
        {
            NetworkChange.NetworkAddressChanged += new
            NetworkAddressChangedEventHandler(AddressChangedCallback);
            Console.WriteLine("Listening for address changes. Press any key to exit.");

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface n in adapters)
            {
                Console.WriteLine("   {0} is {1}", n.Name, n.OperationalStatus);

                IPInterfaceStatistics stats = n.GetIPStatistics();
                IPInterfaceProperties ips = n.GetIPProperties();
            }

            Console.ReadLine();
        }
        static void AddressChangedCallback(object sender, EventArgs e)
        {

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface n in adapters)
            {
                Console.WriteLine("   {0} is {1}", n.Name, n.OperationalStatus);


               
            }
        }


    }
}
