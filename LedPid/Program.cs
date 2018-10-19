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
        private static string UidLcdDisplay = "qzM";
        private static string UidLcdDisplayPID = "jvr";
        private static string UiWheelP = "g5f";
        private static string UiWheelI = "g6A";
        private static string UiWheelD = "g8Q";
        private static BrickletLEDStrip ledStrip;

        private static void Main()
        {
            double setPoint = 400;

            IPConnection ipcon = new IPConnection(); // Create IP connection

            ipcon.Connect(HOST, PORT); // Connect to brickd
            var display = new BrickletLCD20x4(UidLcdDisplay, ipcon);
            var displayPID = new BrickletLCD20x4(UidLcdDisplayPID, ipcon);
            var light = new BrickletAmbientLight(UidAmbientLight, ipcon);
            ledStrip = new BrickletLEDStrip(UidLedStrip, ipcon);
            var pid = new PidCompute(0.3, 0.0001, 0.0000003);
            ledStrip.SetChannelMapping(6);
            var rotaryPotiP = new BrickletRotaryPoti(UiWheelP, ipcon);
            var rotaryPotiI = new BrickletRotaryPoti(UiWheelI, ipcon);
            var rotaryPotiD = new BrickletRotaryPoti(UiWheelD, ipcon);
            var linerPoti = new BrickletLinearPoti("bxu", ipcon);
            display.BacklightOn();
            displayPID.BacklightOn();
        
            var ledHandler = new LedHanlder(ledStrip);
            double P = 0.15;
            double I = 0.15;
            double D = 0.15;

            while (true)
            {
                P = rotaryPotiP.GetAnalogValue() / 100000.0;
                I = rotaryPotiI.GetAnalogValue() / 1000000.0;
                D = rotaryPotiD.GetAnalogValue() / 1000000.0;
                setPoint = linerPoti.GetAnalogValue() / 10;
                
                pid = new PidCompute(P, I, D); 
                Console.WriteLine("Goal: " + setPoint);
                var lux = light.GetIlluminance();
                Console.Out.WriteLine($"Lux: {lux}");
                var res = 0.1* pid.Compute(setPoint, lux);
                Console.Out.WriteLine($"PID Result: {res}");
                ledHandler.SetLedStrip(res);
                display.ClearDisplay();
                displayPID.ClearDisplay();
                display.WriteLine( 0, 0, $"Setpoint: {setPoint}");
                display.WriteLine( 1, 0, $"Lux: {lux}");
                display.WriteLine( 2, 0, $"PID Result: {res}");
                displayPID.WriteLine(0, 0, $"P: {P}");
                displayPID.WriteLine(1, 0, $"I: {I}");
                displayPID.WriteLine(2, 0, $"D: {D}");
                Thread.Sleep(10);
                
       
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            ipcon.Disconnect();
        }
    }
}