using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CCC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (int i = 1; i <= 4; i++)
            {
                RunFor($"input/level1_{i}.in", $"input/output1_{i}.txt");
            }
            Console.ReadKey();
        }

        public static void RunFor(string inputFilename, string outputFilename)
        {

            try
            {
                var lines = File.ReadAllLines(inputFilename);
                int colorsCount = int.Parse(lines[0]);
                var colors = lines.Skip(1);

                var outputList = new List<int>();

                foreach (var color in colors)
                {
                    var rgbValuesAll = color.Split(' ');
                    var firstRgbList = rgbValuesAll.Take(3).ToList();
                    var firstRgb = new Tuple<int, int, int>(int.Parse(firstRgbList[0]), int.Parse(firstRgbList[1]), int.Parse(firstRgbList[2]));
                    var secondRgbList = rgbValuesAll.Skip(3).Take(3).ToList();
                    var secondRgb = new Tuple<int, int, int>(int.Parse(secondRgbList[0]), int.Parse(secondRgbList[1]), int.Parse(secondRgbList[2]));

                    var distance = Math.Sqrt(
                        Math.Pow(firstRgb.Item1 - secondRgb.Item1, 2)
                        + Math.Pow(firstRgb.Item2 - secondRgb.Item2, 2)
                        + Math.Pow(firstRgb.Item3 - secondRgb.Item3, 2)
                    );
                    outputList.Add((int) Math.Floor(distance));
                }

                File.WriteAllLines(outputFilename, outputList.Select(o => o.ToString()));

            } catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
