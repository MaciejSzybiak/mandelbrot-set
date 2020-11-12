using System;
using System.CommandLine;
using System.Security.Cryptography;

namespace mandelbrot_set
{
    class Program
    {
        /// <summary>
        /// Generates an image of Mandelbrot Set.
        /// </summary>
        /// <param name="output">The name of the output image.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="scale">The image scale. Values higher than 1 will zoom in the image.</param>
        /// <param name="maxIter">Maximum iteration count for the escape time algorithm. 
        /// Higher values require more time to generate the image.</param>
        /// <param name="xoffset">Image x offset from the center of the complex plane.</param>
        /// <param name="yoffset">Image y offset from the center of the complex plane.</param>
        /// <param name="threads">The suggested worker threads count.</param>
        /// <param name="benchmark">Runs the built-in benchmark.</param>
        /// <param name="verbose">Set more detailed output.</param>
        static void Main(string output = "mandelbrot", int height = 1000, int width = 1000,
            int scale = 1, int maxIter = 100, double xoffset = 0.5, double yoffset = 0,
            int threads = 8, bool benchmark = false, bool verbose = false)
        {
            scale = scale * width / 3; //make sure the entire fractal fits on screen when the scale is set to 1
            xoffset *= scale;
            yoffset *= scale;
            threads = Math.Clamp(threads, 1, height);
            var generationInfo = new GenerationInfo(height, width, scale, maxIter, xoffset, yoffset);

            if (benchmark)
            {
                RunBenchmark(generationInfo);
            }
            else
            {
                double[,] img = GetGeneratedImage(generationInfo, threads, verbose);
                WriteImageFile(img, generationInfo, GetOutputFilePath(output));
            }
        }

        static double[,] GetGeneratedImage(GenerationInfo generationInfo, int threads, bool verbose)
        {
            Console.WriteLine("Generating mandelbrot set...");

            EscapeTimeGenerator gen = new EscapeTimeGenerator(generationInfo, verbose);
            var img = gen.Generate(threads, out long milliseconds);
            Console.WriteLine($"Finished in {milliseconds} milliseconds.");

            return img;
        }

        static string GetOutputFilePath(string output)
        {
            return output + (output.EndsWith(".bmp") ? "" : ".bmp");
        }

        static void WriteImageFile(double[,] image, GenerationInfo generationInfo, string name)
        {
            Console.Write($"Writing \"{name}\"...");

            ImageGenerator imageGenerator = new ImageGenerator(generationInfo.maxIterations);
            imageGenerator.BitmapFromIntArray(image, generationInfo.resolution).Save($"{name}");

            Console.WriteLine("\tDone.");
        }

        static void RunBenchmark(GenerationInfo generationInfo)
        {
            Benchmark benchmark = new Benchmark();
            benchmark.RunBenchmark(generationInfo);
        }
    }
}
