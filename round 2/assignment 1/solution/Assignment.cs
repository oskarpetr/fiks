namespace Uloha1;

public class Assignment
{
    public Sector StartSector { get; set; } = new();
    public Sector EndSector { get; set; } = new();
    public int InitialSpeed { get; set; }
    public int LowerSpeedLimit { get; set; }
    public int UpperSpeedLimit { get; set; }
    public Sector[,,] Grid { get; set; } = {};
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
    
    public int FindShortestPath()
    {
        // Initialize the open set
        var openSet = new Dictionary<Alternative, int>();
        
        // Initialize the best time
        int bestTime = int.MaxValue;

        // Initialize the beginning
        var beginning = new Alternative()
        {
            Sector = StartSector,
            TotalTime = 0,
            CurrentSpeed = StartSector.CurrentSpeed
        };
        openSet[beginning] = 0 + beginning.CurrentSpeed; 
        
        // A* algorithm
        while (openSet.Count > 0)
        {
            // Get sector with the lowest total time
            Alternative current = openSet.MinBy(x => x.Value).Key;
            openSet.Remove(current);
            
            // If the end sector is reached, return the total time
            if (current.Sector.X == EndSector.X &&
                current.Sector.Y == EndSector.Y &&
                current.Sector.Z == EndSector.Z)
            {
                return bestTime;
            }
            
            // Get neighbors
            var neighbours = GetNeighbors(current);
            
            // Iterate through neighbors
            foreach (var neighbor in neighbours)
            {
                // Total time to reach the neighbor
                var newTime = current.TotalTime + current.CurrentSpeed;
                
                // If the neighbor is the end sector or new best time is reached
                // => update the best time
                if(neighbor.Type == 'E' && bestTime > newTime)
                    bestTime = newTime;

                // Initialize new speed
                var newSpeed = current.CurrentSpeed;
                
                // If the neighbor is a speed sector
                // => update the speed
                if (neighbor.Type == '+' || neighbor.Type == '-')
                    newSpeed = current.CurrentSpeed + neighbor.Acceleration;
                
                // If the new speed is out of bounds
                // => keep the current speed
                if (newSpeed < UpperSpeedLimit || newSpeed > LowerSpeedLimit)
                    newSpeed = current.CurrentSpeed;
                
                // Get the alternative with the new speed
                var alternative = neighbor.Alternatives.FirstOrDefault(x => x.CurrentSpeed == newSpeed);

                // If the alternative does not exist
                // => create a new one
                if (alternative == null)
                {
                    Alternative newAlternative = new()
                    {
                        Sector = neighbor,
                        TotalTime = newTime,
                        CurrentSpeed = newSpeed
                    };
                    
                    neighbor.Alternatives.Add(newAlternative);
                    openSet[newAlternative] = newAlternative.TotalTime + newAlternative.CurrentSpeed;
                }
                
                // If the new time is better than the current one
                // => update the total time
                else if (newTime < alternative.TotalTime)
                {
                    alternative.TotalTime = newTime;
                    
                    // If the alternative is not already in the open set
                    // => add it to the open set
                    if(!openSet.ContainsKey(alternative))
                        openSet[alternative] = alternative.TotalTime + alternative.CurrentSpeed;
                }
            }
        }

        return bestTime;
    }

    public Sector[,,] FillGrid(List<Sector> sectors)
    {
        // Initialize the grid
        Sector[,,] grid = new Sector[Width, Height, Depth];

        // Initialize the grid with default sectors
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    grid[x, y, z] = new()
                    {
                        X = x,
                        Y = y,
                        Z = z,
                        Type = '.',
                        Acceleration = 0,
                        CurrentSpeed = 0,
                        TotalTime = 0,
                    };;
                }
            }
        }

        // Overwrite with significant sectors
        foreach (var sector in sectors)
        {
            grid[sector.X, sector.Y, sector.Z] = sector;
        }

        return grid;
    }
    
    private List<Sector> GetNeighbors(Alternative current)
    {
        // Initialize the neighbors list
        var neighbors = new List<Sector>();
        
        // Initialize the directions
        int[,] directions =
        {
            { 1, 0, 0 }, { -1, 0, 0 }, // X-axis
            { 0, 1, 0 }, { 0, -1, 0 }, // Y-axis
            { 0, 0, 1 }, { 0, 0, -1 }  // Z-axis
        };

        // Iterate through directions
        for (int i = 0; i < 6; i++)
        {
            // Get the new coordinates
            int newX = current.Sector.X + directions[i, 0];
            int newY = current.Sector.Y + directions[i, 1];
            int newZ = current.Sector.Z + directions[i, 2];

            // Check if within bounds
            if (newX >= 0 && newX < Width && newY >= 0 && newY < Height && newZ >= 0 && newZ < Depth)
            {
                var neighbor = Grid[newX, newY, newZ];
                if (neighbor.Type != 'F') // Skip forbidden sectors
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}