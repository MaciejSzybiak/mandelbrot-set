using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace mandelbrot_set
{
    class ImageGenerator
    {
        private int _maxIterations;

        private (byte, byte, byte) GetPixelValue(double arrayValue)
        {
            if (arrayValue == _maxIterations)
            {
                return (0, 0, 0);
            }
            byte val = (byte)(arrayValue / _maxIterations * 255);
            return (val, (byte)(0.5 * val), (byte)(0.5 * val));
        }

        private byte[] ByteDataFromIntArray(double[,] array, (int x, int y) size)
        {
            byte[] bytes = new byte[3 * size.x * size.y];
            int i = 0;
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    var v = GetPixelValue(array[x, y]);
                    bytes[i] = v.Item1;
                    bytes[i + 1] = v.Item2;
                    bytes[i + 2] = v.Item3;
                    i += 3;
                }
            }
            return bytes;
        }

        public Bitmap BitmapFromIntArray(double[,] array, (int x, int y) size)
        {
            var bitmap = new Bitmap(size.x, size.y, PixelFormat.Format24bppRgb);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, size.x, size.y), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte[] pixels = ByteDataFromIntArray(array, size);
            IntPtr ptr = data.Scan0;
            Marshal.Copy(pixels, 0, ptr, size.x * size.y * 3);

            bitmap.UnlockBits(data);
            return bitmap;
        }

        public ImageGenerator(int maxIterations)
        {
            _maxIterations = maxIterations;
        }
    }
}
