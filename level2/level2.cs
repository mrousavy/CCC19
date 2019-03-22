using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CCC
{
    public class Program
    {
        public const int Level = 2;

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
                var cells = lines.Skip(1);

                var outputList = new List<string>();

                foreach (var cell in cells)
                {
                    var rgbValuesAll = cell.Split(' ');


                    int[] distances = new int[dimensions[1] - 1];
                    for (int rgbIndex = 0; rgbIndex < dimensions[1] - 1; rgbIndex++)
                    {
                        var firstRgb = GrabRgb(rgbValuesAll, rgbIndex);
                        var secondRgb = GrabRgb(rgbValuesAll, rgbIndex + 1);

                        var distance = Distance(firstRgb, secondRgb);
                        distances[rgbIndex] = distance;
                    }

                    outputList.Add(string.Join(" ", distances));
                }

                File.WriteAllLines(outputFilename, outputList.Select(o => o.ToString()));

            } catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

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
