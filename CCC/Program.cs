using System;
using System.IO;

namespace CCC
{
    public class Program
    {
        public const string Filename = "input.txt";
        public static void Main(string[] args)
        {
            try
            {
                var lines = File.ReadAllLines(Filename);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    Console.WriteLine($"Line {i}: {line}");
                }

            } catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
