using System.Numerics;
using Uloha2;

string[] file = File.ReadAllLines("input.txt");

int crewCount = int.Parse(file[0]);
string[] output = new string[crewCount];
List<Tree> trees = new();

// Parse crews
int currentLine = 1;

for (int i = 0; i < crewCount; i++)
{
    string[] crewInfo = file[currentLine].Split(" ");
    ulong[] pointsLine = file[currentLine + 1].Split(" ").Select(ulong.Parse).ToArray();
    int[] supervisorLine = file[currentLine + 2].Split(" ").Select(int.Parse).ToArray();
    
    List<Member> members = new();
    for (int j = 0; j < int.Parse(crewInfo[0]); j++)
    {
        members.Add(new Member()
        {
            Id = j,
            Points = pointsLine[j],
            Supervisor = supervisorLine[j]
        });
    }
    
    Member crewCaptain = members.FirstOrDefault(x => x.Supervisor == -1);
    Node rootNode = BuildTree(crewCaptain, members);
    
    Tree tree = new Tree()
    {
        Points = ulong.Parse(crewInfo[1]),
        NodeCount = int.Parse(crewInfo[0]),
        RootNode = rootNode
    };
    
    trees.Add(tree);
    currentLine += 3;
}

for (int i = 0; i < crewCount; i++)
{
    var tree = trees[i];
    string result = FixSupervisors(tree, tree.RootNode);
    
    if (result == "ajajaj")
    {
        output[i] = "ajajaj";
    }
    else
    {
        var comparer = new NodeComparer();
        var queue = new SortedSet<Node>(comparer);
        PopulateQueue(tree.RootNode, queue);
        
        Maximise(tree, queue);
        
        output[i] = FindSmallestPoint(tree.RootNode).ToString();
    }
    
    File.AppendAllText("output.txt", output[i] + Environment.NewLine);
}

void PopulateQueue(Node node, SortedSet<Node> queue)
{
    queue.Add(node);
    
    foreach (var child in node.Children)
    {
        PopulateQueue(child, queue);
    }
}

void Maximise(Tree tree, SortedSet<Node> queue)
{
    while (tree.Points > 0)
    {
        Node node = queue.Min;
        queue.Remove(node);

        var pointsNeeded = 0;
        var currentNode = node;

        while (currentNode.Supervisor != null && currentNode.Points + 1 == currentNode.Supervisor.Points)
        {
            pointsNeeded++;
            currentNode = currentNode.Supervisor;
        }

        if (pointsNeeded + 1 > tree.Points) return;

        currentNode = node.Supervisor;
        while (pointsNeeded > 0)
        {
            pointsNeeded--;
            tree.Points--;

            queue.Remove(currentNode);
            currentNode.Points++;

            queue.Add(currentNode);

            currentNode = currentNode.Supervisor;
        }

        node.Points++;
        tree.Points--;
        queue.Add(node);
    }
}

Node BuildTree(Member supervisor, List<Member> members, Node parent = null)
{
    Node currentNode = new Node
    {
        Id = supervisor.Id,
        Points = supervisor.Points,
        Supervisor = parent,
        Children = Array.Empty<Node>() 
    };

    int supervisorIndex = members.IndexOf(supervisor) + 1;

    List<Member> subordinates = members.Where(x => x.Supervisor == supervisorIndex).ToList();

    List<Node> children = new();
    foreach (var subordinate in subordinates)
    {
        children.Add(BuildTree(subordinate, members, currentNode));
    }

    currentNode.Children = children.ToArray();

    return currentNode;
}


string FixSupervisors(Tree tree, Node supervisor)
{
    foreach (var child in supervisor.Children)
    {
        var result = FixSupervisors(tree, child);
        
        if(result == "ajajaj") return "ajajaj";
        
        // if child has more points
        if (child.Points >= supervisor.Points)
        {
            BigInteger newPoints = supervisor.Children.Max(x => x.Points) + 1;
            BigInteger pointsOffset = newPoints - supervisor.Points;
            
            if(tree.Points < pointsOffset) return "ajajaj";
            
            tree.Points -= pointsOffset;
            supervisor.Points = newPoints;
        }
    }

    return "";
}

BigInteger FindSmallestPoint(Node rootNode)
{
    BigInteger min = rootNode.Points;
    
    foreach (var child in rootNode.Children)
    {
        BigInteger childMin = FindSmallestPoint(child);
        if (childMin < min) min = childMin;
    }

    return min;
}