using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CCC
{
    public class Program
    {
        public const int Level = 4;
        public static List<Tuple<int, int>> Visited = new List<Tuple<int, int>>();

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

                int numberOfPaths = int.Parse(lines[1]);

                var paths = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();
                for (int i = 0; i < numberOfPaths; i++)
                {
                    var startingPoints = lines[i + 2].Split(' ').Select(int.Parse).ToArray();
                    paths.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(
                        new Tuple<int, int>(startingPoints[0], startingPoints[1]),
                        new Tuple<int, int>(startingPoints[2], startingPoints[3])
                        ));
                }

                var cells = lines.Skip(2 + numberOfPaths).Select(l => l.Split(' ')).ToList();


                for (int i = 0; i < numberOfPaths; i++)
                {
                    int count = 0;
                    FindPathCount(rows, columns, paths[i].Item1, paths[i].Item2, cells, ref count);
                    Console.WriteLine($"Count: {count}");
                }

                var outputList = new List<string>();


                File.WriteAllLines(outputFilename, outputList.Select(o => o.ToString()));
                Console.WriteLine($"Wrote {outputFilename}");
            } catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static int FindPathCount(int rows, int columns, Tuple<int, int> from, Tuple<int, int> to, List<string[]> cells, ref int count)
        {
            if (from.Item1 == to.Item1 && from.Item2 == to.Item2)
            {
                return -1;
            }

            int row = from.Item1;
            int col = from.Item2;


            Visited.Add(from);
            var rgbValuesThisRow = cells[row];
                var thisRgb = GrabRgb(rgbValuesThisRow, col);
                var distances = FindDistances(rows, columns, cells, thisRgb, row, col);

                var notVisited = new Dictionary<Tuple<int, int>, int>();
                foreach (var distance in distances)
                {
                    if (!Visited.Any(v => v.Item1 == distance.Key.Item1 && v.Item2 == distance.Key.Item2))
                    {
                        notVisited.Add(distance.Key, distance.Value);
                    }
                }

                if (!notVisited.Any())
                {
                    Console.WriteLine("ALL VISITED!");
                    break;
                }

                var paths = new List<int>();
                foreach (var next in notVisited)
                {
                    int path = FindPathCount(rows, columns, next.Key, to, cells, ref count);
                    if (path != -1)
                    {
                        paths.Add(path);
                    }
                }

                count += paths.Min();
                return paths.Min();

            return -1;
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
                    if (thatRgb.Item1 != 0 && thatRgb.Item2 != 0 && thatRgb.Item3 != 0)
                    {
                        int distance = Distance(thisRgb, thatRgb);
                        distances.Add(new Tuple<int, int>(row, col), distance);
                    }
                }
            }

            {
                int row = thisRow;
                int col = thisCol - 1;
                if (col >= 0)
                {
                    var rgbValuesThisRow = rgbValuesList[row];
                    var thatRgb = GrabRgb(rgbValuesThisRow, col);
                    if (thatRgb.Item1 != 0 && thatRgb.Item2 != 0 && thatRgb.Item3 != 0)
                    {
                        int distance = Distance(thisRgb, thatRgb);
                        distances.Add(new Tuple<int, int>(row, col), distance);
                    }
                }
            }

            {
                int row = thisRow + 1;
                int col = thisCol;
                if (row < rows)
                {
                    var rgbValuesThisRow = rgbValuesList[row];
                    var thatRgb = GrabRgb(rgbValuesThisRow, col);
                    if (thatRgb.Item1 != 0 && thatRgb.Item2 != 0 && thatRgb.Item3 != 0)
                    {
                        int distance = Distance(thisRgb, thatRgb);
                        distances.Add(new Tuple<int, int>(row, col), distance);
                    }
                }
            }

            {
                int row = thisRow - 1;
                int col = thisCol;
                if (row >= 0)
                {
                    var rgbValuesThisRow = rgbValuesList[row];
                    var thatRgb = GrabRgb(rgbValuesThisRow, col);
                    if (thatRgb.Item1 != 0 && thatRgb.Item2 != 0 && thatRgb.Item3 != 0)
                    {
                        int distance = Distance(thisRgb, thatRgb);
                        distances.Add(new Tuple<int, int>(row, col), distance);
                    }
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
