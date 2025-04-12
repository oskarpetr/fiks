using Uloha1;

// Load input file
string[] file = File.ReadAllLines("input.txt");

// Assignment count
int assignments = int.Parse(file[0]);

// Current line in the file
int currentLine = 1;

for (int i = 0; i < assignments; i++)
{
    // Parse speed limits and initial speed
    string[] firstLine = file[currentLine].Split(" ");
    int upperSpeedLimit = int.Parse(firstLine[0]);
    int lowerSpeedLimit = int.Parse(firstLine[1]);
    int initialSpeed = int.Parse(firstLine[2]);

    // Parse grid dimensions and sector count
    string[] secondLine = file[currentLine + 1].Split(" ");
    int width = int.Parse(secondLine[0]);
    int height = int.Parse(secondLine[1]);
    int depth = int.Parse(secondLine[2]);
    int sectorCount = int.Parse(secondLine[3]);

    // Initialize sectors
    var sectors = new List<Sector>();
    Sector startSector = new();
    Sector endSector = new();

    // Read and process sectors
    for (int j = 0; j < sectorCount; j++)
    {
        // Parse sector data
        string[] sectorLine = file[currentLine + 2 + j].Split(" ");
        Sector sector = new()
        {
            X = int.Parse(sectorLine[0]),
            Y = int.Parse(sectorLine[1]),
            Z = int.Parse(sectorLine[2]),
            Type = sectorLine[3][0],
            Acceleration = 0,
            CurrentSpeed = initialSpeed,
            TotalTime = 0
        };

        // If acceleration is provided, parse it
        if (sector.Type == '+' || sector.Type == '-')
            sector.Acceleration = int.Parse(sectorLine[3]);

        // Identify the start and end sectors
        if (sector.Type == 'B') startSector = sector;
        else if (sector.Type == 'E') endSector = sector;

        sectors.Add(sector);
    }

    // Create the assignment
    Assignment assignment = new()
    {
        UpperSpeedLimit = upperSpeedLimit,
        LowerSpeedLimit = lowerSpeedLimit,
        InitialSpeed = initialSpeed,
        Width = width,
        Height = height,
        Depth = depth,
        StartSector = startSector,
        EndSector = endSector
    };
    
    // Fill the grid with sectors
    assignment.Grid = assignment.FillGrid(sectors);

    // Find the shortest path
    int shortestTime = assignment.FindShortestPath();

    // Write the result to the output file
    File.AppendAllText("output.txt", shortestTime + Environment.NewLine);

    // Move to the next assignment
    currentLine += sectorCount + 2;
}