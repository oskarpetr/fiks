using System.Text;

static void ProcessFile(string inputFile, string outputFile)
{
    StringBuilder output = new StringBuilder();

    using (var reader = new StreamReader(inputFile))
    {
        int assignments = int.Parse(reader.ReadLine());
        
        for (int i = 0; i < assignments; i++)
        {
            int starCount = int.Parse(reader.ReadLine());
            List<(int x, int y)> stars = new List<(int x, int y)>();

            string xLine = reader.ReadLine();
            string yLine = reader.ReadLine();
            string[] xCoordinates = xLine.Split(' ');
            string[] yCoordinates = yLine.Split(' ');

            for (int j = 0; j < starCount; j++)
            {
                int x = int.Parse(xCoordinates[j]);
                int y = int.Parse(yCoordinates[j]);
                stars.Add((x, y));
            }

            string center = FindCenter(stars);
            output.AppendLine(center);
        }
    }

    File.WriteAllText(outputFile, output.ToString());
}

static string FindCenter(List<(int x, int y)> points)
{
    if (points.Count % 2 != 0)
    {
        return "ajajaj";
    }

    double sumX = 0;
    double sumY = 0;
    
    foreach (var point in points)
    {
        sumX += point.x;
        sumY += point.y;
    }

    double centerX = sumX / points.Count;
    double centerY = sumY / points.Count;
    
    var pointSet = new HashSet<(int x, int y)>(points);

    foreach (var point in points)
    {
        var symmetricPoint = ((int)(2 * centerX - point.x), (int)(2 * centerY - point.y));
        if (!pointSet.Contains(symmetricPoint))
            return "ajajaj";
    }

    return $"{centerX:F6} {centerY:F6}".Replace(",", ".");
}

ProcessFile("input.txt", "output.txt");