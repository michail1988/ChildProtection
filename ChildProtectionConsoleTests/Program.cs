﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Management;
using System.Runtime.InteropServices;

namespace ChildProtectionConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.Write("------ Child Protection Tests ------");

            string userName = WindowsIdentity.GetCurrent().Name;
            System.Console.Write("Zalogowany użytkownik: " + userName);


            WindowsIdentity loggedUser = WindowsIdentity.GetCurrent();

            //https://msdn.microsoft.com/en-us/library/windows/desktop/aa394102%28v=vs.85%29.aspx
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName, Domain, Workgroup FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
            string domain = (string)collection.Cast<ManagementBaseObject>().First()["Domain"];
            string workgroup = (string)collection.Cast<ManagementBaseObject>().First()["Workgroup"];

            // Force reboot
            //ExitWindowsEx(4, 0);

            System.Console.WriteLine("Firewall Enabled: " + Firewall.IsFirewallEnabled());

            string gazetaPl = "80.252.0.145";
            Firewall.BlockIP(gazetaPl);

            System.Console.WriteLine("Połączenia internetowe zablokowane. Wciśnij klawisz, by odblokować.");

            System.Console.ReadKey();

            System.Console.WriteLine("Press something to stop the timer.");

            Timer.Tick();

            System.Console.Write("------ ---------------------- ------");
        }

        //Force: http://stackoverflow.com/questions/24726116/when-using-exitwindowsex-logoff-works-but-shutdown-and-restart-do-not
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        public static bool WindowsLogOff()
        {
            return ExitWindowsEx(0, 0);
        }


        [Flags]
        public enum ExitWindows : uint
        {
            // ONE of the following five:
            LogOff = 0x00,
            ShutDown = 0x01,
            Reboot = 0x02,
            PowerOff = 0x08,
            RestartApps = 0x40,
            // plus AT MOST ONE of the following two:
            Force = 0x04,
            ForceIfHung = 0x10,
        }
        [Flags]
        enum ShutdownReason : uint
        {
            MajorApplication = 0x00040000,
            MajorHardware = 0x00010000,
            MajorLegacyApi = 0x00070000,
            MajorOperatingSystem = 0x00020000,
            MajorOther = 0x00000000,
            MajorPower = 0x00060000,
            MajorSoftware = 0x00030000,
            MajorSystem = 0x00050000,

            MinorBlueScreen = 0x0000000F,
            MinorCordUnplugged = 0x0000000b,
            MinorDisk = 0x00000007,
            MinorEnvironment = 0x0000000c,
            MinorHardwareDriver = 0x0000000d,
            MinorHotfix = 0x00000011,
            MinorHung = 0x00000005,
            MinorInstallation = 0x00000002,
            MinorMaintenance = 0x00000001,
            MinorMMC = 0x00000019,
            MinorNetworkConnectivity = 0x00000014,
            MinorNetworkCard = 0x00000009,
            MinorOther = 0x00000000,
            MinorOtherDriver = 0x0000000e,
            MinorPowerSupply = 0x0000000a,
            MinorProcessor = 0x00000008,
            MinorReconfig = 0x00000004,
            MinorSecurity = 0x00000013,
            MinorSecurityFix = 0x00000012,
            MinorSecurityFixUninstall = 0x00000018,
            MinorServicePack = 0x00000010,
            MinorServicePackUninstall = 0x00000016,
            MinorTermSrv = 0x00000020,
            MinorUnstable = 0x00000006,
            MinorUpgrade = 0x00000003,
            MinorWMI = 0x00000015,

            FlagUserDefined = 0x40000000,
            FlagPlanned = 0x80000000
        }

        ///http://security.stackexchange.com/questions/30985/create-a-unterminable-process-in-windows
    }
}
