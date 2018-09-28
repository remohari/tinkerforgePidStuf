using System;
using System.Threading;
using Tinkerforge;

namespace LedPid
{
    internal class Program
    {
        private static string HOST = "localhost";
        private static int PORT = 4223;
        private static string UidLedStrip = "wSR";
        private static string UidAmbientLight = "jxW";
        private static BrickletLEDStrip ledStrip;

        private static void Main()
        {
            double setPoint = 130;

            IPConnection ipcon = new IPConnection(); // Create IP connection

            ipcon.Connect(HOST, PORT); // Connect to brickd

            var light = new BrickletAmbientLight(UidAmbientLight, ipcon);
            ledStrip = new BrickletLEDStrip(UidLedStrip, ipcon);
            var pid = new PidCompute(0.3, 0.00001, 0);
            ledStrip.SetChannelMapping(6);

            var ledHandler = new LedHanlder(ledStrip);

            while (true)
            {
                var lux = light.GetIlluminance();
                Console.Out.WriteLine($"Lux: {lux}");
                var res = pid.Compute(setPoint, lux);
                Console.Out.WriteLine($"PID Result: {res}");
                ledHandler.SetLedStrip(res);
                Thread.Sleep(20);
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            ipcon.Disconnect();
        }
    }
}