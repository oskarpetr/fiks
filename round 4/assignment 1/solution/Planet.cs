using System.Numerics;

namespace Uloha1;

public class Planet
{
    public BigInteger CellNumber { get; set; }
    public BigInteger? Q { get; set; }
    public BigInteger? R { get; set; }
    public BigInteger? S => -Q - R;
}