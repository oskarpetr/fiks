namespace Uloha3;

public class Node
{
    public int TankIndex { get; set; }
    public long Volume { get; set; }
    public List<Node> Children { get; set; } = new();
    public Node? Parent { get; set; }
}