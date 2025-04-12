namespace Uloha1;

public class Sector
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public int Acceleration { get; set; }
    public char Type { get; set; }
    public int TotalTime { get; set; }
    public int CurrentSpeed { get; set; }
    public List<Alternative> Alternatives { get; set; } = new();
}