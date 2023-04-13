using System;
using System.Management;

namespace DomainInfoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt the user for domain credentials
            Console.Write("Enter domain username: ");
            string username = Console.ReadLine();
            Console.Write("Enter domain password: ");
            string password = Console.ReadLine();

            // Connect to the domain controller using WMI
            ConnectionOptions options = new ConnectionOptions();
            options.Username = username;
            options.Password = password;
            options.Authority = $"ntlmdomain:{Environment.UserDomainName}";

            ManagementScope scope = new ManagementScope($"\\\\{Environment.UserDomainName}\\root\\CIMV2", options);
            scope.Connect();

            // Retrieve information about running processes
            ObjectQuery processQuery = new ObjectQuery("SELECT * FROM Win32_Process");
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher(scope, processQuery);
            ManagementObjectCollection processCollection = processSearcher.Get();

            foreach (ManagementObject process in processCollection)
            {
                Console.WriteLine($"Process Name: {process["Name"]}, Process ID: {process["ProcessId"]}");
            }

            // Retrieve information about registered users and devices
            ObjectQuery userQuery = new ObjectQuery("SELECT * FROM Win32_UserAccount");
            ManagementObjectSearcher userSearcher = new ManagementObjectSearcher(scope, userQuery);
            ManagementObjectCollection userCollection = userSearcher.Get();

            foreach (ManagementObject user in userCollection)
            {
                Console.WriteLine($"User Name: {user["Name"]}, Domain: {user["Domain"]}, SID: {user["SID"]}");
            }

            ObjectQuery deviceQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher deviceSearcher = new ManagementObjectSearcher(scope, deviceQuery);
            ManagementObjectCollection deviceCollection = deviceSearcher.Get();

            foreach (ManagementObject device in deviceCollection)
            {
                Console.WriteLine($"Device Name: {device["Name"]}, Manufacturer: {device["Manufacturer"]}, Model: {device["Model"]}");
            }

            Console.ReadKey();
        }
    }
}
