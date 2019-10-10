using System;
using System.Threading.Tasks;
using Speedlimits;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var speedlimit = new Speedlimit(30, TimeSpan.FromSeconds(2));
            
            var random = new Random();
            Drive(speedlimit, random, 0);
            Drive(speedlimit, random, 1);
            Drive(speedlimit, random, 2);
            Drive(speedlimit, random, 3);
            Drive(speedlimit, random, 4);
            Drive(speedlimit, random, 5);
            Drive(speedlimit, random, 6);
            Drive(speedlimit, random, 7);
            Drive(speedlimit, random, 8);
            await Drive(speedlimit, random, 9);
        }

        private static async Task Drive(Speedlimit speedlimit, Random random, int no)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(random.Next(30, 100)));
                await speedlimit.ObeyAsync();
                Console.Write(no);
            }
        }
    }
}