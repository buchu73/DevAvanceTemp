using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int width = 4000;  // Image width
            int height = 4000; // Image height

            // Define start and end colors for the gradient
            Color startColor = Color.Red;
            Color endColor = Color.Blue;

            // Create the pattern image
            Bitmap patterntImage = CreateCheckerboardImage(width, height, 4);

            // Save the image as a BMP file
            string filePath = "pattern.bmp";
            patterntImage.Save(filePath);

            Console.WriteLine($"pattern image saved as {filePath}");


            //// Create the gradient image
            //Bitmap gradientImage = CreateGradientImage(width, height, startColor, endColor);

            //// Save the image as a BMP file
            //string filePath = "gradient.bmp";
            //gradientImage.Save(filePath);

            //Console.WriteLine($"Gradient image saved as {filePath}");
        }

        static Bitmap CreateGradientImage(int width, int height, Color startColor, Color endColor)
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Calculate the interpolation factor
                    double t = (double)x / (width - 1);

                    // Interpolate between the start and end colors
                    int r = (int)(startColor.R + t * (endColor.R - startColor.R));
                    int g = (int)(startColor.G + t * (endColor.G - startColor.G));
                    int b = (int)(startColor.B + t * (endColor.B - startColor.B));

                    // Set the pixel color
                    Color interpolatedColor = Color.FromArgb(r, g, b);
                    bitmap.SetPixel(x, y, interpolatedColor);
                }
            }

            return bitmap;
        }

        static Bitmap CreatePatternImage(int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                Color color = (y / 100) % 2 == 0 ? Color.White : Color.Black;
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }

        static Bitmap CreateCheckerboardImage(int width, int height, int squareSize)
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Determine color based on the current square's position
                    bool isBlack = ((x / squareSize) % 2 == (y / squareSize) % 2);
                    Color color = isBlack ? Color.Black : Color.White;

                    // Set the pixel color
                    bitmap.SetPixel(x, y, color);
                }
            }

            return bitmap;
        }
    }
}
