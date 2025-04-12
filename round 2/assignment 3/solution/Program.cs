using Uloha3;

// Load data
var data = LoadData();

// Execute commands
ExecuteCommands(data);

// Load data
List<Assignment> LoadData()
{
    using StreamReader reader = new StreamReader("input.txt");
    
    // Read assignments
    int assignmentsCount = int.Parse(reader.ReadLine()!);
    List<Assignment> assignments = new();

    // Load assignments
    for (int i = 0; i < assignmentsCount; i++)
    {
        // Initialize vars
        List<Command> commands = new();
        Node rootNode = new();

        // Load tank count
        int tankCount = int.Parse(reader.ReadLine()!);
        Dictionary<int, Node> dictionary = new();

        // Load nodes
        for (int j = 0; j < tankCount; j++)
        {
            Node node = new()
            {
                TankIndex = j,
                Volume = long.Parse(reader.ReadLine()!)
            };

            // Add node to dictionary
            dictionary.Add(j, node);
            
            // Set root node
            if (j == 0) rootNode = node;
        }

        // Load children
        for (int j = 0; j < tankCount; j++)
        {
            var children = reader.ReadLine()!.Split(" ").Select(int.Parse).ToList();

            // Remove child count
            children.RemoveAt(0);

            var node = dictionary[j];
            
            // Add children
            foreach (var childIndex in children)
            {
                var child = dictionary[childIndex];
                child.Parent = node;
                node.Children.Add(child);
            }
        }

        // Load commands
        int commandCount = int.Parse(reader.ReadLine()!);
        for (int j = 0; j < commandCount; j++)
        {
            var commandLine = reader.ReadLine()!.Split(" ");

            // Create command
            Command command = new()
            {
                Type = commandLine[0] switch
                {
                    "!" => CommandType.ExclamationPoint,
                    "?" => CommandType.QuestionMark,
                    "#" => CommandType.NumberSign,
                    _ => CommandType.ExclamationPoint
                },
                TankIndex = int.Parse(commandLine[1])
            };

            // Add command
            commands.Add(command);
        }

        // Create assignment
        Assignment assignment = new()
        {
            RootNode = rootNode,
            Dictionary = dictionary,
            Commands = commands
        };

        // Add assignment
        assignments.Add(assignment);
    }

    return assignments;
}

// Execute commands
void ExecuteCommands(List<Assignment> assignments)
{
    // Assignments
    foreach (var assignment in assignments)
    {
        List<string> output = new();

        // Execute command
        foreach (var command in assignment.Commands)
        {
            switch (command.Type)
            {
                // ExclamationPoint
                case CommandType.ExclamationPoint:
                    ExclamationPoint(assignment.Dictionary, command.TankIndex);
                    break;
                
                // QuestionMark
                case CommandType.QuestionMark:
                    var questionMark = QuestionMark(assignment.Dictionary, command.TankIndex);
                    output.Add(questionMark.ToString());
                    break;
                
                // NumberSign
                case CommandType.NumberSign:
                    var numberSign = NumberSign(assignment.Dictionary, command.TankIndex);
                    output.Add(numberSign.ToString());
                    break;
            }
        }
        
        File.AppendAllLines("output.txt", output);
    }
}

// Exclamation point
void ExclamationPoint(Dictionary<int, Node> dictionary, int tankIndex)
{
    var node = dictionary[tankIndex];
    ApplyVolumeReduction(node);
}

// Helper function for ExclamationPoint
void ApplyVolumeReduction(Node node)
{
    // Reduce volume
    node.Volume -= (long)Math.Ceiling(node.Volume / 2.0);

    // Apply to children
    foreach (var child in node.Children)
    {
        ApplyVolumeReduction(child);
    }
}

// Question mark
long QuestionMark(Dictionary<int, Node> dictionary, int tankIndex)
{
    return dictionary[tankIndex].Volume;
}

// Number sign
long NumberSign(Dictionary<int, Node> dictionary, int tankIndex)
{
    return CalculateSubtreeVolume(dictionary[tankIndex]);
}

// Helper function for NumberSign
long CalculateSubtreeVolume(Node node)
{
    // Accumulate volume
    long totalVolume = node.Volume;

    // Calculate subtree volume
    foreach (var child in node.Children)
    {
        totalVolume += CalculateSubtreeVolume(child);
    }

    return totalVolume;
}