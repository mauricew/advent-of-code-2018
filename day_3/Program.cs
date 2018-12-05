using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aoc_Day3
{

    /*
     * This whole thing is embarassingly unoptimized.
     * That's just weekday programming.
     */
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<Rectangle> rects = lines.Select(Rectangle.Parse).ToList();

            Dictionary<(int x, int y), List<int>> overlaps = new Dictionary<(int x, int y), List<int>>();

            foreach (Rectangle r in rects)
            {
                for (int x = r.OffsetLeft; x < r.OffsetRight; x++)
                {
                    for (int y = r.OffsetTop; y < r.OffsetBottom; y++)
                    {
                        if (!overlaps.ContainsKey((x, y)))
                        {
                            overlaps.Add((x, y), new List<int>() { r.Id });
                        }
                        else
                        {
                            overlaps[(x, y)].Add(r.Id);
                        }
                    }
                }
            }

            IEnumerable<IList<int>> overlapPoints = overlaps.Values.Where(x => x.Count > 1);
            IEnumerable<IList<int>> nonOverlapPoints = overlaps.Values.Where(x => x.Count == 1);

            Console.WriteLine(overlapPoints.Count());

            IEnumerable<int> overlapIds = overlapPoints.SelectMany(x => x);
            IEnumerable<int> nonOverlapIds = nonOverlapPoints.SelectMany(x => x);

            Rectangle noOverlaps = rects.Single(r =>
                nonOverlapIds.Contains(r.Id) &&
                !overlapIds.Contains(r.Id)
            );
            Console.WriteLine($"Rectangle #{noOverlaps.Id} has no overlaps");

            Console.ReadKey();
        }
    }

    class Rectangle
    {
        public int Id { get; set; }
        public int OffsetLeft { get; set; }
        public int OffsetTop { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int OffsetRight => OffsetLeft + Width;
        public int OffsetBottom => OffsetTop + Height;

        public int Area => Width * Height;

        public override bool Equals(object obj)
        {
            return Id == (obj as Rectangle)?.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static Rectangle Parse(string inputStr)
        {
            int id = int.Parse(inputStr.Split('#')[1].Split(' ')[0]);

            string[] offsets = inputStr.Split('@')[1].Split(':')[0].Split(',');
            int offsetLeft = int.Parse(offsets[0]);
            int offsetTop = int.Parse(offsets[1]);

            string[] dims = inputStr.Split(':')[1].Split('x');
            int width = int.Parse(dims[0]);
            int height = int.Parse(dims[1]);

            return new Rectangle
            {
                Id = id,
                OffsetLeft = offsetLeft,
                OffsetTop = offsetTop,
                Width = width,
                Height = height
            };
        }
    }
}
