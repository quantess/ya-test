using System;
using System.Globalization;
using System.IO;
using System.Linq;
using TaskTwo.Models;

namespace TaskTwo
{
    class Program
    {
        static int CalcCoeff (double percent)
        {
            if (percent >= 60)
            {
                return 1;
            }
            else if (percent >= 30)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        static void Main(string[] args)
        {
            var delimeter = '\t';
            var dateFormat = $"yyyy-MM-dd HH:mm:ss";

            //var stats = new List<(DateTime, DateTime, short)>();

            var data = File.ReadAllLines(@"..\\..\\..\\..\\..\\data\\файл 2.csv")
                ?.Skip(1)
                ?.AsParallel()
                ?.Select(x => x.Split(delimeter))
                ?.Select(x => new AssessorJudge {
                    Login = x[0],
                    UId = ushort.Parse(x[1], CultureInfo.InvariantCulture),
                    DocId = ushort.Parse(x[2], CultureInfo.InvariantCulture),
                    Judgement = Convert.ToBoolean(short.Parse(x[3], CultureInfo.InvariantCulture)),
                    CorrectJudgement = Convert.ToBoolean(short.Parse(x[4], CultureInfo.InvariantCulture))
                })
                .ToList();

            var groupsByDoc = data.GroupBy(x => x.DocId)
                .Select(x => new {
                    DocId = x.Key,
                    Coeff = CalcCoeff(x.Sum(y =>
                        y.IsCorrect ? 1d : 0d) / x.Count() * 100)
                })
                .ToDictionary(x => x.DocId);

            var result = data.Select(x => new {
                    UId = x.UId,
                    Cost = (x.IsCorrect ? 1d : 0d) * groupsByDoc[x.DocId].Coeff
                })
                .GroupBy(x => x.UId)
                .Select(x => new
                {
                    UId = x.Key,
                    Rating = x.Average(y => y.Cost)
                })
                .OrderBy(x => x.Rating)
                .ToList();

            var num = 1;
            Console.WriteLine("Top 5 users");

            foreach (var res in result.Take(5))
            {
                Console.WriteLine($"{num}. {res.UId}(uid) => {res.Rating:N5}");
                num++; 
            }

            Console.ReadKey();
        }
    }
}
