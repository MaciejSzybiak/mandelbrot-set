using System;

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
        static void Main(string output = "mandelbrot", int height = 1000, int width = 1000,
            int scale = 1, int maxIter = 100, double xoffset = 0.5, double yoffset = 0, int threads = 8)
        {
            //make sure the entire fractal fits on screen when the scale is set to 1
            scale = scale * width / 3;

            Console.WriteLine("Generating mandelbrot set...");

            EscapeTimeGenerator gen = new EscapeTimeGenerator(maxIter, scale, (width, height), (xoffset * scale, yoffset * scale));
            double[,] img = gen.Generate(Math.Clamp(threads, 1, 100));

            string name = output + (output.EndsWith(".bmp") ? "" : ".bmp");
            Console.Write($"Writing \"{name}\"...");

            ImageGenerator imageGenerator = new ImageGenerator(maxIter);
            imageGenerator.BitmapFromIntArray(img, (width, height)).Save($"{name}");

            Console.WriteLine("\tDone.");
        }
    }
}
