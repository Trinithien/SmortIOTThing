﻿// See https://aka.ms/new-console-template for more information
//using static System.Net.Mime.MediaTypeNames;
//using System.Drawing;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

//using System;

double temperature;
Console.WriteLine("Arduino: /dev/ttyACM0");
Console.WriteLine("Available Ports:");
//List<Sensor> sensors = new List<Sensor>();

//using var db = new SensorContext();


foreach (string s in SerialPort.GetPortNames())
{
    Console.WriteLine("{0}", s);
}
// comPort: /dev/ttyACM0 // Should be
//string comPort = "/dev/ttyACM0";
// Baudrate: 9600 // Should be // Actually is 10000
int baudrate = 9600;
Console.WriteLine("Select comport: ");
string? comPort = Console.ReadLine(); // Comport velg meg!
if (comPort == "test" || comPort == "Test")
{

    using var db = new SensorContext();
    Console.WriteLine("testing");


    List<Sensor> sensors = new List<Sensor>();
    sensors.Add(new Sensor { Timestamp = DateTimeOffset.Now, Value = 20.0, Name = "stringName", Unit = "degris Kelvin" });

    db.Add(new Sensor { Timestamp = DateTimeOffset.Now, Value = 20.0, Name = "stringName", Unit = "degris Kelvin" });
    db.SaveChanges();

    Console.WriteLine(JsonSerializer.Serialize(sensors).ToString());

    //sleep();
    StartClient(20.0);

    //JsonSerializer.Serialize("sensors").ToString();
    //return 0;
    // IP: 10.38.42.92:1024
}

static void StartClient(double value)
{

    using var db = new SensorContext();
    byte[] bytes = new byte[1024];
    try
    {
        IPHostEntry host = Dns.GetHostEntry("");
        IPAddress ipAddress = IPAddress.Parse("192.168.130.102");
        System.Console.WriteLine(ipAddress.ToString());
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1024);

        //IPAddress.Parse("10.38.42.92");
        Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

        try
        {
            sender.Connect(remoteEP);

            Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint?.ToString());


            List<Sensor> sensors = new List<Sensor>();
            sensors.Add(new Sensor { Timestamp = DateTimeOffset.Now, Value = value, Name = "stringName", Unit = "degris Kelvin" });
            db.Add(new Sensor { Timestamp = DateTimeOffset.Now, Value = value, Name = "stringName", Unit = "degris Kelvin" });
            db.SaveChanges();

            //JsonSerializer.Serialize("sensors");
            byte[] msg = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(sensors) + "<EOF>");

            int bytesSent = sender.Send(msg);

            int bytesRec = sender.Receive(bytes);
            Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        catch (ArgumentNullException ane)
        {
            Console.WriteLine("ArgumentNullException: {0}", ane.ToString());
        }
        catch (SocketException se)
        {
            Console.WriteLine("SocketException: {0}", se.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }

}

try
{
    using (SerialPort port = new SerialPort(comPort!, baudrate, Parity.None))
    {
        //byte[] b = new byte[3];
        //b[0] = (byte)Convert.ToInt32("255");
        //b[1] = (byte)Convert.ToInt32("255");
        //b[2] = (byte)Convert.ToInt32("255");
        port.ReadTimeout = 3000;
        port.WriteTimeout = 3000;
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
                if (string.IsNullOrWhiteSpace(message) == false)
                {
                    temperature = Convert.ToDouble(message);
                    StartClient(temperature);
                }
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

