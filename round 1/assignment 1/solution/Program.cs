using Uloha1;

string[] file = File.ReadAllLines("input.txt");
int assignmentsCount = int.Parse(file[0]);

List<Assignment> assignments = new();

int lastLine = 1;
for (int i = 0; i < assignmentsCount; i++)
{
    if (assignments.Count > 0)
    {
        lastLine += assignments.Last().Points.Count + 1;
    }

    int currentLine = assignments.Count == 0 ? 1 : lastLine;
    string[] firstLineSplit = file[currentLine].Split(" ");
    int pointsCount = int.Parse(firstLineSplit[0]);

    List<Point> points = new List<Point>();
    for (int j = 0; j < pointsCount; j++)
    {
        string[] positions = file[currentLine + j + 1].Split(" ");
        Point point = new Point()
        {
            X = int.Parse(positions[0]),
            Y = int.Parse(positions[1]),
            Z = int.Parse(positions[2]),
        };
        points.Add(point);
    }
    
    Assignment assignment = new()
    {
        VelocityHigh = int.Parse(firstLineSplit[1]),
        VelocityStale = int.Parse(firstLineSplit[2]),
        VelocityLow = int.Parse(firstLineSplit[3]),
        Points = points
    };
    
    assignments.Add(assignment);
}

string filePath = "output.txt";
string[] content = new string[assignments.Count];
for (int i = 0; i < assignments.Count; i++)
{
    double time = 0;
    for (int j = 0; j < assignments[i].Points.Count - 1; j++)
    {
        Point point1 = assignments[i].Points[j];
        Point point2 = assignments[i].Points[j + 1];

        double distance = Math.Sqrt(Math.Pow((point2.X - point1.X), 2) + Math.Pow((point2.Y - point1.Y), 2) +
                                   Math.Pow((point2.Z - point1.Z), 2));
        int velocity;
        if (point2.Z > point1.Z)
        {
            velocity = assignments[i].VelocityHigh;
        } else if (point2.Z == point1.Z)
        {
            velocity = assignments[i].VelocityStale;
        }
        else
        {
            velocity = assignments[i].VelocityLow;
        }
        
        time += distance / velocity;
    }

    string fixedTime = time.ToString("0.00000").Replace(",", ".");
    content[i] = fixedTime;
}

File.WriteAllLines(filePath, content);