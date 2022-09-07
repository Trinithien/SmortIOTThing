using System;
namespace SmortIOTThing.Desktop.Interfaces
{
    public interface ITemperatureSensorStatus
    {
        event EventHandler StatusChanged;
    }
}
