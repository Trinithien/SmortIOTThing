// See https://aka.ms/new-console-template for more information
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.IO.Ports;

Console.WriteLine("Hello, World!");
try
{
    using (SerialPort port = new SerialPort("Comport VELG MEG !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", 10000, Parity.None))
    {
        byte[] b = new byte[3];
        b[0] = (byte)Convert.ToInt32("255");
        b[1] = (byte)Convert.ToInt32("255");
        b[2] = (byte)Convert.ToInt32("255");
        port.Open();
        port.Write(b, 0, 3);
        while (port.BytesToRead < 3) {}
        b = new byte[3];
        port.Read(b, 0, 3);
        string b0 = b[0].ToString();
        string b1 = b[1].ToString();
        string b2 = b[2].ToString();
        Console.WriteLine($"{b0},{b1},{b2}");
    }

}
catch (Exception ee)
{
    Console.WriteLine(ee.Message);
}