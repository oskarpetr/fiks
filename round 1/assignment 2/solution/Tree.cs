using System.Numerics;

namespace Uloha2;

public class Tree
{
    public Node RootNode { get; set; }
    public int NodeCount { get; set; }
    public BigInteger Points { get; set; }
    public Member[] Members { get; set; }
}