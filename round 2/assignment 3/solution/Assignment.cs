namespace Uloha3;

public class Assignment
{
    public Node RootNode { get; set; } = new();
    public Dictionary<int, Node> Dictionary { get; set; } = new();
    public List<Command> Commands { get; set; } = new();
}