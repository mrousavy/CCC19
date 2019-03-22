using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CCC
{
    public class Program
    {
        public const int Level = 3;

        public static void Main(string[] args)
        {
            for (int i = 1; i <= 4; i++)
            {
                RunFor($"input/level{Level}_{i}.in", $"input/output{Level}_{i}.txt");
            }
            Console.ReadKey();
        }

        public static void RunFor(string inputFilename, string outputFilename)
        {

            try
            {
                var lines = File.ReadAllLines(inputFilename);
                var dimensions = lines[0].Split(' ').Select(int.Parse).ToList();
                int rows = dimensions[0], columns = dimensions[1];
                var startingPoints = lines[1].Split(' ').Select(int.Parse).ToList();
                int startRow = startingPoints[0], startColumn = startingPoints[1];
                var cells = lines.Skip(2).Select(l => l.Split(' ')).ToList();

                var outputList = new List<string>();
                var visited = new List<Tuple<int, int>>();

                int row = startRow;
                int col = startColumn;

                visited.Add(new Tuple<int, int>(row, col));
                outputList.Add($"{row} {col}");

                while (true)
                {
                    var rgbValuesThisRow = cells[row];
                    var thisRgb = GrabRgb(rgbValuesThisRow, col);
                    var distances = FindDistances(rows, columns, cells, thisRgb, row, col);


                    var notVisited = new Dictionary<Tuple<int, int>, int>();
                    foreach (var distance in distances)
                    {
                        if (!visited.Any(v => v.Item1 == distance.Key.Item1 && v.Item2 == distance.Key.Item2))
                        {
                            notVisited.Add(distance.Key, distance.Value);
                        }
                    }
                    if (!notVisited.Any())
                        break;

                    //Console.WriteLine(string.Join(" ", notVisited));

                    var ordered = notVisited
                        .OrderBy(pair => pair.Value)
                        .ThenBy(pair => pair.Key.Item1)
                        .ThenBy(pair => pair.Key.Item2)
                        .ToList();
                    var first = ordered.First();
                    var min = first.Key;
                    var second = ordered.Skip(1).FirstOrDefault();
                    if (first.Value == second.Value)
                        Console.WriteLine($"Same! {first} {second}");

                    //var min = notVisited
                    //    .OrderBy(pair => pair.Key.Item2)
                    //    .Aggregate((c, d) =>
                    //        c.Value < d.Value
                    //            ? c : d).Key;

                    visited.Add(min);
                    outputList.Add($"{min.Item1} {min.Item2}");
                    row = min.Item1;
                    col = min.Item2;
                }

                File.WriteAllLines(outputFilename, outputList.Select(o => o.ToString()));
                Console.WriteLine($"Wrote {outputFilename}");
            } catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static Dictionary<Tuple<int, int>, int> FindDistances(int rows, int columns, List<string[]> rgbValuesList, Tuple<int, int, int> thisRgb, int thisRow, int thisCol)
        {
            // Tuple<[x,y], distance>
            var distances = new Dictionary<Tuple<int, int>, int>();


            {
                int row = thisRow;
                int col = thisCol + 1;
                if (col < columns)
                {
                    var rgbValuesThisRow = rgbValuesList[row];
                    var thatRgb = GrabRgb(rgbValuesThisRow, col);
                    int distance = Distance(thisRgb, thatRgb);
                    distances.Add(new Tuple<int, int>(row, col), distance);
                }
            }

            {
                int row = thisRow + 1;
                int col = thisCol;
                if (row < rows)
                {
                    var rgbValuesThisRow = rgbValuesList[row];
                    var thatRgb = GrabRgb(rgbValuesThisRow, col);
                    int distance = Distance(thisRgb, thatRgb);
                    distances.Add(new Tuple<int, int>(row, col), distance);
                }
            }

            {
                int row = thisRow - 1;
                int col = thisCol;
                if (row >= 0)
                {
                    var rgbValuesThisRow = rgbValuesList[row];
                    var thatRgb = GrabRgb(rgbValuesThisRow, col);
                    int distance = Distance(thisRgb, thatRgb);
                    distances.Add(new Tuple<int, int>(row, col), distance);
                }
            }

            return distances;
        }

        //public static Dictionary<Tuple<int, int>, int> FindDistances(int rows, int columns, List<string[]> rgbValuesList, Tuple<int, int, int> thisRgb)
        //{
        //    // Tuple<[x,y], distance>
        //    var distances = new Dictionary<Tuple<int, int>, int>();
        //    for (int row = 0; row < rows; row++)
        //    {
        //        var rgbValuesThisRow = rgbValuesList[row];
        //        for (int col = 0; col < columns; col++)
        //        {
        //            var thatRgb = GrabRgb(rgbValuesThisRow, col);
        //            int distance = Distance(thisRgb, thatRgb);
        //            distances.Add(new Tuple<int, int>(row, col), distance);
        //        }
        //    }

        //    return distances;
        //}

        public static int Distance(Tuple<int, int, int> firstRgb, Tuple<int, int, int> secondRgb)
        {
            var distance = Math.Sqrt(
                Math.Pow(firstRgb.Item1 - secondRgb.Item1, 2)
                + Math.Pow(firstRgb.Item2 - secondRgb.Item2, 2)
                + Math.Pow(firstRgb.Item3 - secondRgb.Item3, 2)
            );
            return (int) Math.Floor(distance);
        }

        public static Tuple<int, int, int> GrabRgb(string[] allValues, int rgbIndex)
        {
            int skip = rgbIndex * 3;
            var tmp = allValues.Skip(skip).Take(3).ToList();
            return new Tuple<int, int, int>(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]));
        }
    }
}
