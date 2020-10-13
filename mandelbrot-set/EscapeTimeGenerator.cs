using System;
using System.Numerics;

namespace mandelbrot_set
{
    public class EscapeTimeGenerator
    {
        private int _maxIterations;
        private double _scale;
        private (int x, int y) _resolution;
        private (double x, double y) _offset;
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

        public double[,] Generate()
        {
            double xOffs = _resolution.x / 2 + _offset.x;
            double yOffs = _resolution.y / 2 + _offset.y;
            for (int x = 0; x < _resolution.x; x++)
            {
                for (int y = 0; y < _resolution.y; y++)
                {
                    Complex num = new Complex((x - xOffs) * _scale, (y - yOffs) * _scale);
                    _image[x, y] = GetIterationCount(num);
                }
            }

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
    }
}
