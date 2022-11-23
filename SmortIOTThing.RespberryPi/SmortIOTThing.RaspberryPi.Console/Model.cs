using Microsoft.EntityFrameworkCore;

public class SensorContext : DbContext
{
    public DbSet<Sensor> Sensors { get; set; }
    public string DbPath { get; }

    public SensorContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "sensor.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Sensor
{
    public int SensorId { get; set; }
    public double Value { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Name { get; set; } = null!;
    public string Unit { get; set; } = null!;
}
