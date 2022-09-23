using SmortIOTThing.Desktop.EventArguements;
using SmortIOTThing.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop
{
    public class SocketTemperatureSensorStatus :ITemperatureSensorStatus
    {
        public event EventHandler StatusChanged = null!;
        public SocketTemperatureSensorStatus()
        {
            _ = OpenSocket();
        }
        public virtual void OnStatusChanged(TemperatureSensorEventArgs e)
        {
            EventHandler handler = StatusChanged;
            handler?.Invoke(this, e);
        }
        
        private async Task OpenSocket()
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork,
                                             SocketType.Stream,
                                             ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("10.38.42.92"), 1024);
            listenSocket.Bind(ep);
            listenSocket.Listen(10);

            // Incoming data from the client.
            Socket handler = listenSocket;
            while (true)
            {
                string data = null;
                byte[] bytes = null;
                await Task.Run(() =>
                {
                    handler = listenSocket.Accept();

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            data = data.Substring(0, data.Length - 5);
                            break;
                        }
                    }
                });
                var points = JsonSerializer.Deserialize<List<SensorResponse>>(data);
                var sensors = points.Select(point => point.Name).Distinct();
                var series = new List<SensorSerie>();
                foreach (var sensor in sensors)
                {
                    series.Add(
                        new SensorSerie
                        {
                            Name = sensor,
                            SensorPoints = points.Where(point => point.Name == sensor)
                                                 .Select(point => new SensorPoint { Value = point.Value, Timestamp = point.Timestamp })
                                                 .ToList(),
                            Unit = points.Where(point => point.Name == sensor).First().Unit
                        }
                        ) ;
                }
                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                var e = new TemperatureSensorEventArgs
                {
                    SensorSeries = series.ToArray()
                };
                OnStatusChanged(e);
            }

        }
    }
}

