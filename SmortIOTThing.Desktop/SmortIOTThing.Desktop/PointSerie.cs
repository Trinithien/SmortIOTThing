using System.Collections.Generic;

namespace SmortIOTThing.Desktop;

public class SensorSerie
{
    public string Name { get; set; }
    public string Unit {  get; set; }
    public List<SensorPoint> SensorPoints { get; set; }
}
