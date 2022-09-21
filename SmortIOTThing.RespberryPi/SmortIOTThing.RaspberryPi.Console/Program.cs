// See https://aka.ms/new-console-template for more information
//using static System.Net.Mime.MediaTypeNames;
//using System.Drawing;
using System.IO.Ports;

Console.WriteLine("Available Ports:");
foreach (string s in SerialPort.GetPortNames())
{
    Console.WriteLine("   {0}", s);
}
// comPort: /dev/ttyACM0 // Should be
// Baudrate: 9600 // Should be // Actually is 10000
int baudrate = 9600;
Console.WriteLine("Select comport: ");
string? comPort = Console.ReadLine(); // Comport velg meg!
try
{
    using (SerialPort port = new SerialPort(comPort!, baudrate, Parity.None))
    {
        byte[] b = new byte[3];
        b[0] = (byte)Convert.ToInt32("255");
        b[1] = (byte)Convert.ToInt32("255");
        b[2] = (byte)Convert.ToInt32("255");
        port.ReadTimeout = 1500;
        port.WriteTimeout = 1500;
        port.Open();
        //port.Write(b, 0, 3);
        //while (port.BytesToRead < 3) {}
        //b = new byte[3];
        //port.Read(b, 0, 3);
        try
        {
            while (true)
            {
                string message = port.ReadLine();
                Console.WriteLine(message);
            }
        }
        catch (TimeoutException)
        {
        }
        port.Close();
        // string b0 = b[0].ToString();
        // string b1 = b[1].ToString();
        // string b2 = b[2].ToString();
        //Console.WriteLine($"{b0},{b1},{b2}");
    }

}
catch (Exception ee)
{
    Console.WriteLine(ee.Message);
}
