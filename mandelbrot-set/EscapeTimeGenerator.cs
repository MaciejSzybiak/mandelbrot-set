using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace mandelbrot_set
{
    class EscapeTimeGenerator
    {
        private int _maxIterations;
        private double _scale;
        private (int x, int y) _resolution;
        private (double x, double y) _offset;
        double xOffs = 0;
        double yOffs = 0;
        private readonly double[,] _image;

        private double GetIterationCount(Complex c)
        {
            Complex z = 0;
            int n = 0;

            while (z.Magnitude <= 2 && n < _maxIterations) {
                z = z * z + c;
                n++;
            }
            if (n == _maxIterations)
            {
                return n;
            }
        
            return n + 1 - Math.Log(Math.Log2(z.Magnitude));
        }

        private double[,] GenerateImageTask(int startIndex, int endIndex)
        {
            double[,] array = new double[_resolution.x, endIndex - startIndex];
            for (int x = 0; x < _resolution.x; x++)
            {
                for (int y = startIndex; y < endIndex; y++)
                {
                    Complex num = new Complex((x - xOffs) * _scale, (y - yOffs) * _scale);
                    array[x, y - startIndex] = GetIterationCount(num);
                }
            }
            return array;
        }

        private void RunTasks(int count)
        {
            int taskSize = _resolution.y / count;

            List<Task<double[,]>> tasks = new List<Task<double[,]>>();
            List<(int, int)> indices = new List<(int, int)>();

            //start the tasks
            for(int i = 0; i < count; i++)
            {
                int start = i * taskSize;
                int end = start + taskSize;
                if(end + taskSize > _resolution.y)
                {
                    end = _resolution.y;
                }
                var newTask = Task.Run(() => GenerateImageTask(start, end));
                Console.WriteLine($"Worker thread ID: {newTask.Id}\t{(end - start) *_resolution.x} pixels");
                tasks.Add(newTask);
                indices.Add((start, end));
            }

            //put the results back together
            for(int j = 0; j < tasks.Count; j++)
            {
                double[,] result = tasks[j].Result;
                var range = indices[j];
                for(int x = 0; x < _resolution.x; x++)
                {
                    for(int y = range.Item1; y < range.Item2; y++)
                    {
                        _image[x, y] = result[x, y - range.Item1];
                    }
                }
            }
        }

        public double[,] Generate(int threadCount)
        {
            xOffs = _resolution.x / 2 + _offset.x;
            yOffs = _resolution.y / 2 + _offset.y;

            var timer = System.Diagnostics.Stopwatch.StartNew();

            RunTasks(threadCount);

            timer.Stop();
            Console.WriteLine($"Finished in {timer.ElapsedMilliseconds} milliseconds.");

            return _image;
        }

	    public EscapeTimeGenerator(int maxIterations, double scale, (int x, int y) resolution, (double x, double y) offset)
        {
            _maxIterations = maxIterations;
            _resolution = resolution;
            _scale = 1/scale;
            _offset = offset;
            _image = new double[resolution.x, resolution.y];
        }

        public EscapeTimeGenerator(GenerationInfo info)
        {
            _maxIterations = info.maxIterations;
            _resolution = info.resolution;
            _scale = 1 / info.scale;
            _offset = info.offset;
            _image = new double[_resolution.x, _resolution.y];
        }
    }
}
