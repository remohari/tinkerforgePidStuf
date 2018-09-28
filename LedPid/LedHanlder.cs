using System;
using Tinkerforge;

namespace LedPid
{
    public class LedHanlder
    {
        private BrickletLEDStrip ledStrip;
        private byte currentBrightnes;

        public LedHanlder(BrickletLEDStrip ledStrip)
        {
            this.ledStrip = ledStrip;
            currentBrightnes = 100;
        }

        public void SetLedStrip(double res)
        {
            byte brightnes = currentBrightnes;
            var bla = (brightnes + (res));
            if (bla < 0)
            {
                bla = 0;
            }
            else if (bla > 255)
            {
                bla = 255;
            }
            Console.Out.WriteLine($"LED Difference: {(byte)(res)}");
            Console.Out.WriteLine($"LED new Brightnes: {bla}");
            byte size = 16;
            byte[] r = new byte[size];
            byte[] g = new byte[size];
            byte[] b = new byte[size];
            var brightnesValueAsByte = (byte)bla;
            currentBrightnes = brightnesValueAsByte;
            for (int i = 0; i < size; i++)
            {
                r[i] = brightnesValueAsByte;
                g[i] = brightnesValueAsByte;
                b[i] = brightnesValueAsByte;
            }

            for (int i = 0; i < 4; i++)
            {
                ledStrip.SetRGBValues(i * (byte)16, 16, r, g, b);
            }
        }
    }
}