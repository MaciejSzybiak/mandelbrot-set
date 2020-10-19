using System;
using System.Collections.Generic;
using System.Text;

namespace mandelbrot_set
{
    class Benchmark
    {
        private static readonly int[] threadCounts = { 1, 2, 4, 8, 12, 16, 24, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
        private const int benchmarkCount = 5;
        private readonly long[] timers = new long[threadCounts.Length];

        public void RunBenchmark(GenerationInfo generationInfo)
        {
            EscapeTimeGenerator generator = new EscapeTimeGenerator(generationInfo, false);

            Console.WriteLine("Benchmarking...\n");
            Console.WriteLine("Threads\t|  Average duration");
            Console.WriteLine("----------------------------");
            for(int i = threadCounts.Length - 1; i >= 0; i--)
            {
                long sum = 0;
                Console.Write($"  {threadCounts[i]}");
                for (int j = 0; j < benchmarkCount; j++)
                {
                    generator.Generate(threadCounts[i], out long result);
                    sum += result;
                }
                timers[i] = sum / benchmarkCount;
                
                Console.WriteLine($"\t|\t{timers[i]}ms");
            }
        }
    }
}
