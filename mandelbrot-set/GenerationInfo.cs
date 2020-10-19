using System;
using System.Collections.Generic;
using System.Text;

namespace mandelbrot_set
{
    struct GenerationInfo
    {
        public (int x, int y) resolution;
        public double scale;
        public int maxIterations;
        public (double x, double y) offset;

        public GenerationInfo(int height, int width, double scale, int maxIterations, double xoffset, double yoffset)
        {
            resolution.y = height;
            resolution.x = width;
            this.scale = scale;
            this.maxIterations = maxIterations;
            offset.x = xoffset;
            offset.y = yoffset;
        }
    }
}
