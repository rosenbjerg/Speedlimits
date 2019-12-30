using System;
using System.Threading.Tasks;
using Speedlimits;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var speedlimit = new RateLimiter(30, TimeSpan.FromSeconds(2));
            
            var random = new Random();
            RandomlySpam(speedlimit, random, 0);
            RandomlySpam(speedlimit, random, 1);
            RandomlySpam(speedlimit, random, 2);
            RandomlySpam(speedlimit, random, 3);
            RandomlySpam(speedlimit, random, 4);
            RandomlySpam(speedlimit, random, 5);
            RandomlySpam(speedlimit, random, 6);
            RandomlySpam(speedlimit, random, 7);
            RandomlySpam(speedlimit, random, 8);
            await RandomlySpam(speedlimit, random, 9);
        }

        private static async Task RandomlySpam(RateLimiter rateLimiter, Random random, int no)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(random.Next(30, 100)));
                await rateLimiter.WaitAsync();
                Console.Write(no);
            }
        }
    }
}