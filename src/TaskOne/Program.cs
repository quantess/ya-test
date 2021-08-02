using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TaskOne.Models;

namespace TaskOne
{
    class Program
    {
        static void Main(string[] args)
        {
            var delimeter = '\t';
            var dateFormat = $"yyyy-MM-dd HH:mm:ss";

            var stats = new List<(DateTime, DateTime, short)>();

            var data = File.ReadAllLines(@"..\\..\\..\\..\\..\\data\\файл 1.txt")
                ?.Skip(1)
                ?.AsParallel()
                ?.Select(x => x.Split(delimeter))
                ?.Select(x => new AssessorTask {
                    Login = x[0],
                    TaskId = decimal.Parse(x[1], CultureInfo.InvariantCulture),
                    Microtasks = (short)decimal.Parse(x[2], CultureInfo.InvariantCulture),
                    AssignedTs = DateTime.ParseExact(x[3], dateFormat, CultureInfo.InvariantCulture),
                    ClosedTs = DateTime.ParseExact(x[4], dateFormat, CultureInfo.InvariantCulture)
                })
                .Where(x => x.AssignedTs <= x.ClosedTs)
                .ToList();


            var groupsByLogin = data.GroupBy(x => x.Login);

            foreach (var gr in groupsByLogin)
            {
                var sortedGroup = gr.OrderBy(x => x.ClosedTs)
                    .ToList();

                DateTime? minDate = (DateTime?)null;
                DateTime? maxDate = (DateTime?)null;
                short sumMicrotasks = 0;

                for (int i = 0; i < sortedGroup.Count(); i++)
                {
                    var t = sortedGroup[i];

                    if (minDate == null)
                        minDate = sortedGroup[i].AssignedTs;

                    if (maxDate == null)
                        maxDate = sortedGroup[i].ClosedTs;

                    var isIntersect = sortedGroup[i].DateTimeRange
                        .Intersects(new DateTimeRange(minDate.Value, maxDate.Value));

                    if (isIntersect)
                    {
                        if (minDate > sortedGroup[i].AssignedTs)
                            minDate = sortedGroup[i].AssignedTs;

                        if (maxDate < sortedGroup[i].ClosedTs)
                            maxDate = sortedGroup[i].ClosedTs;

                        sumMicrotasks += sortedGroup[i].Microtasks;
                    }
                    else
                    {
                        stats.Add(new(minDate.Value, maxDate.Value, sumMicrotasks));

                        minDate = sortedGroup[i].AssignedTs;
                        maxDate = sortedGroup[i].ClosedTs;
                        sumMicrotasks = sortedGroup[i].Microtasks;
                    }
                }
            }

            var avgCost = stats
                .Select(x => (x.Item2 - x.Item1).TotalSeconds / x.Item3)
                .Select(x => x / 30)
                .Average();

            Console.WriteLine($"Average cost is {avgCost:N0}N per microtask");
            Console.ReadKey();
        }
    }
}
