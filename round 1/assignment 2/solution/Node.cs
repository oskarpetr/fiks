using System.Numerics;

namespace Uloha2;

public class Node
{
    public int Id { get; set; }
    public BigInteger Points { get; set; }
    public Node? Supervisor { get; set; }
    public Node[] Children { get; set; }
}

class NodeComparer : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        int result = x.Points.CompareTo(y.Points);
        if (result != 0)
            return result;

        return x.Id.CompareTo(y.Id);
    }
}