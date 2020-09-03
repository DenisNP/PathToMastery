using System;
using System.Collections.Generic;
using System.Linq;
using PathToMastery.Models.State;

namespace PathToMastery.Services
{
    public class VillageService
    {
        private static readonly int[] Lines = {2, 3, 7, 8, 8, 5, 2};
        private const int MaxLevel = 3;
        private const int X0 = 4;
        private const int Y0 = 4;
        private const int NewPagodaPointRange = 3;

        public Village GenerateVillage(List<Path> paths, int seed)
        {
            var random = new Random(seed);
            var points = Lines
                .SelectMany((maxY, x) => Enumerable.Range(0, maxY).Select(y => new Point(x + 1, y + 1, X0, Y0)))
                .OrderBy(p => p.Dist)
                .ToList();
            
            var pagodas = new List<Pagoda>();
            var pathMetas = new List<PathMeta>();
            
            foreach (var path in paths)
            {
                var nonEmptyDays = path.Days
                    .Where(d => d.Type != DateType.N && d.Type != DateType.Link && d.Type != DateType.Checkpoint)
                    .ToList();

                // calculate last unbreakable chain length
                var daysLast = nonEmptyDays.AsEnumerable()
                    .Reverse()
                    .TakeWhile(d => d.Type == DateType.Done || d.Type == DateType.DoneLink)
                    .Count();
                
                pathMetas.Add(path.Data.ToMeta(daysLast));
                
                // generate pagodas
                foreach (var day in nonEmptyDays)
                {
                    var isBreak = day.Type == DateType.Break;
                    var pagodasOfColor = pagodas.Where(p => p.Color == path.Data.Color).ToList();
                    if (isBreak && pagodasOfColor.Count > 0)
                    {
                        // on break destroy random pagoda
                        var pIndex = random.Next(0, pagodasOfColor.Count);
                        pagodas.Remove(pagodasOfColor[pIndex]);
                    }
                    else if (!isBreak)
                    {
                        // level up random pagoda or create a new one
                        var levelUp = random.Next(100) <= 25;
                        var notMaxPagodas = pagodasOfColor.Where(p => p.Level < MaxLevel).ToList();
                        if ((levelUp || points.Count == 0) && notMaxPagodas.Count > 0)
                        {
                            var nmIndex = random.Next(0, notMaxPagodas.Count);
                            notMaxPagodas[nmIndex].Level++;
                        }
                        else if (points.Count > 0)
                        {
                            // create a new one
                            var pointIndex = random.Next(0, Math.Min(NewPagodaPointRange, points.Count));
                            var point = points[pointIndex];
                            points.RemoveAt(pointIndex);

                            var pagoda = new Pagoda
                            {
                                X = point.X,
                                Y = point.Y,
                                Color = path.Data.Color,
                                Level = 1
                            };
                            pagodas.Add(pagoda);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return new Village
            {
                Pagodas = pagodas.OrderBy(p => p.X).ThenBy(p => p.Y).ToList(),
                Paths = pathMetas
            };
        }
    }

    internal class Point
    {
        public int X { get; }
        public int Y { get; }
        public double Dist { get; }

        public Point(int x, int y, int x0, int y0)
        {
            X = x;
            Y = y;

            Dist = Math.Sqrt((x - x0) * (x - x0) + (y - y0) * (y - y0));
        }
    }
}