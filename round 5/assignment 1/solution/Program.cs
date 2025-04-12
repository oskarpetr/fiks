using Uloha1;

const string inputPath = "input.txt";
const string outputPath = "output.txt";

LoadAssignments();

return;

void LoadAssignments()
{
    using var reader = new StreamReader(inputPath);
    using var writer = new StreamWriter(outputPath);
    
    var assignmentCount = int.Parse(reader.ReadLine()!);
    
    for (var i = 0; i < assignmentCount; i++)
    {
        var stringCount = int.Parse(reader.ReadLine()!);
        string[] strings = new string[stringCount];
        
        for(var j = 0; j < stringCount; j++)
        {
            strings[j] = reader.ReadLine()!;
        }
        
        Assignment assignment = new()
        {
            Strings = strings
        };
        
        long count = Solve(assignment);
        writer.WriteLine(count);

        writer.Flush();
    }

    writer.Close();
    reader.Close();
}

long Solve(Assignment assignment)
{
    var countMap = new Dictionary<string, int>();
    foreach (var s in assignment.Strings)
    {
        if (countMap.ContainsKey(s)) countMap[s]++;
        else countMap[s] = 1;
    }

    long result = 0;

    foreach (var group in countMap)
        result += (long)group.Value * (group.Value - 1) / 2;

    var sortedStrings = assignment.Strings.Distinct().OrderBy(s => s.Length).ToList();

    for (int i = 0; i < sortedStrings.Count; i++)
    {
        string current = sortedStrings[i];
        for (int j = i + 1; j < sortedStrings.Count; j++)
        {
            string longer = sortedStrings[j];
            if (longer.Contains(current))
            {
                result += (long)countMap[current] * countMap[longer];
            }
        }
    }

    return result;
}