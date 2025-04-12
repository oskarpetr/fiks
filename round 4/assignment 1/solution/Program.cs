using System.Numerics;
using Uloha1;

// read input
string[] file = File.ReadAllLines("input.txt");

List<string> output = new List<string>();

// parse assignments
int assignmentsCount = int.Parse(file[0]);
for (int i = 0; i < assignmentsCount; i++)
{
    var planets = file[i + 1].Split(" ").Select(x => BigInteger.Parse(x));
    var assignment = new Assignment()
    {
        Planets = planets.Select(x => new Planet() { CellNumber = x }).ToList()
    };
    
    var result = ProcessAssignment(assignment);
    output.Add(result.ToString());
}

// write output
File.WriteAllLines("output.txt", output);

// process assignments
BigInteger ProcessAssignment(Assignment assignment)
{
    foreach (var planet in assignment.Planets)
    {
        DetermineQ(planet);
        DetermineR(planet);
    }

    var distances = new List<BigInteger>();
    for (int i = 0; i < assignment.Planets.Count; i++)
    {
        var planet1 = assignment.Planets[i];
        var planet2 = assignment.Planets[(i + 1) % assignment.Planets.Count];

        var existsStraightLine = ExistsStraightLine(planet1, planet2);
        if (!existsStraightLine) return -1;
        
        var distance = Distance(planet1, planet2);
        distances.Add(distance);
    }

    var equalDistances = distances.All(x => x == distances[0]);
    if (!equalDistances) return -1;

    var hasCenter = (distances[0] - 1) % 3 == 0;
    if (!hasCenter) return 0;
    
    var centerCoordinates = GetCenterCoordinates(assignment.Planets[0], assignment.Planets[1], assignment.Planets[2]);
    GetCenter(centerCoordinates);
    
    return centerCoordinates.CellNumber;
}

// determine q coordinate
void DetermineQ(Planet planet)
{
    var ring = DetermineRing(planet);
    var ringSize = DetermineRingSize(ring);
    var maxCellInRing = MaxCellInRing(ring);
    
    var cellInRingIndex = planet.CellNumber - (maxCellInRing - ringSize) - 1;
    BigInteger qCoordinate;
    
    var reverseDirection = ring % 2 != 0;
    var ringBaseLength = RingBaseLength(ring);
    var startingPoint = ringBaseLength / -2;

    if (!reverseDirection)
    {
        if (cellInRingIndex < ringSize - ring)
        {
            qCoordinate = startingPoint + cellInRingIndex;
        }
        else
        {
            qCoordinate = ring - 1;
        }
    }
    else
    {
        if (cellInRingIndex < ring)
        {
            qCoordinate = ring - 1;
        }
        else
        {
            qCoordinate = -startingPoint - (cellInRingIndex - ring + 1);
        }
    }
    
    planet.Q = qCoordinate;
}

// determine r coordinate
void DetermineR(Planet planet)
{
    var ring = DetermineRing(planet);
    var ringSize = DetermineRingSize(ring);
    var maxCellInRing = MaxCellInRing(ring);
    
    var cellInRingIndex = planet.CellNumber - (maxCellInRing - ringSize) - 1;
    BigInteger rCoordinate;
    
    if (cellInRingIndex < ring - 1)
    {
        rCoordinate = cellInRingIndex;
    }
    else if (cellInRingIndex >= ring - 1 && cellInRingIndex <= ringSize - ring)
    {
        rCoordinate = ring - 1;
    }
    else
    {
        rCoordinate = ringSize - cellInRingIndex - 1;
    }
    
    planet.R = -rCoordinate;
}

// determine ring
BigInteger DetermineRing(Planet planet)
{
    BigInteger discriminant = 1 + 24 * planet.CellNumber;
    BigInteger sqrtDiscriminant = BigIntSqrt(discriminant);

    // If the square root is not exact, round up to the next integer
    if (sqrtDiscriminant * sqrtDiscriminant < discriminant)
    {
        sqrtDiscriminant += 1;
    }

    // Add 1 to the square root and divide by 6, emulating ceiling
    BigInteger numerator = sqrtDiscriminant + 1;
    BigInteger ring = numerator / 6;

    // If there's a remainder, increment the result to emulate ceiling
    if (numerator % 6 != 0)
    {
        ring += 1;
    }

    return ring;
}


// determine size of ring
BigInteger DetermineRingSize(BigInteger ring)
{
    return (3 * ring) - 2;
}

// determine max cell in ring
BigInteger MaxCellInRing(BigInteger ring)
{
    return (3 * ring * ring - ring) / 2;
}

// determine base length of ring
BigInteger RingBaseLength(BigInteger ring)
{
    return 2 * ring - 1;
}

// check if two planets are in straight line
bool ExistsStraightLine(Planet planet1, Planet planet2)
{
    return planet1.Q == planet2.Q || planet1.R == planet2.R || planet1.S == planet2.S;
}

// determine distance between two planets
BigInteger Distance(Planet planet1, Planet planet2)
{
    return new[] {
        BigInteger.Abs((planet2.Q ?? 0) - (planet1.Q ?? 0)), 
        BigInteger.Abs((planet2.R ?? 0) - (planet1.R ?? 0)),
        BigInteger.Abs((planet2.S ?? 0) - (planet1.S ?? 0))
    }.Max() + 1;
}

// get center coordinates of assignment
Planet GetCenterCoordinates(Planet planet1, Planet planet2, Planet planet3)
{
    var qCoordinate = (planet1.Q + planet2.Q + planet3.Q) / 3;
    var rCoordinate = (planet1.R + planet2.R + planet3.R) / 3;
    
    return new Planet() { Q = qCoordinate, R = rCoordinate };
}

// get center of planet
void GetCenter(Planet planet)
{
    var ring = DetermineRingByCoordinates(planet);
    var firstInRing = GetFirstInRing(ring);
    
    var reverseDirection = ring % 2 != 0;

    if (reverseDirection)
    {
        if (planet.Q == ring - 1)
        {
            planet.CellNumber = firstInRing + BigInteger.Abs(planet.R ?? 0);
        }
        else if (planet.R == -(ring - 1))
        {
            planet.CellNumber = firstInRing + ring + ((planet.S ?? 0) - 1);
        }
        else
        {
            planet.CellNumber = firstInRing + (2 * ring - 1) + BigInteger.Abs((planet.Q ?? 0) + 1);
        }
    }
    else
    {
        if (planet.S == ring - 1)
        {
            planet.CellNumber = firstInRing + BigInteger.Abs(planet.R ?? 0);
        }
        else if (planet.R == -(ring - 1))
        {
            planet.CellNumber = firstInRing + ring + ((planet.Q ?? 0) - 1);
        }
        else
        {
            planet.CellNumber = firstInRing + (2 * ring - 1) + BigInteger.Abs((planet.S ?? 0) + 1);
        }
    }
}

// determine ring by coordinates
BigInteger DetermineRingByCoordinates(Planet planet)
{
    return new[] {
        BigInteger.Abs(planet.Q ?? 0), 
        BigInteger.Abs(planet.R ?? 0),
        BigInteger.Abs(planet.S ?? 0)
    }.Max() + 1;
}

// get first cell in ring
BigInteger GetFirstInRing(BigInteger ring)
{
    return MaxCellInRing(ring - 1) + 1;
}

// utility function for BigInteger square root
BigInteger BigIntSqrt(BigInteger value)
{
    if (value == 0) return 0;
    BigInteger root = value / 2;
    BigInteger lastRoot = 0;
    while (root != lastRoot)
    {
        lastRoot = root;
        root = (root + value / root) / 2;
    }
    return root;
}