using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputImagePath = "../../input/pattern.bmp"; // Input BMP image

            string outputFileName = string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(inputImagePath) ,Guid.NewGuid());
            string compressedFilePath = $"../../output/{outputFileName}.rle"; // Output RLE compressed file
            string decompressedImagePath = $"../../output/{outputFileName}.bmp"; // Decompressed BMP image

            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                byte[] imageData = File.ReadAllBytes(inputImagePath);
                List<byte> compressedData = CompressRLE(imageData);
                File.WriteAllBytes(compressedFilePath, compressedData.ToArray());

                byte[] decompressedData = DecompressRLE(compressedData);
                File.WriteAllBytes(decompressedImagePath, decompressedData);

                sw.Stop();
                Console.WriteLine("Compression and decompression completed in {0} seconds.", sw.Elapsed.TotalSeconds);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        // Compress using RLE
        static List<byte> CompressRLE(byte[] data)
        {
            List<byte> compressed = new List<byte>();
            int n = data.Length;
            if (n == 0) return compressed;

            byte current = data[0];
            byte length = 1;

            for (int i = 1; i < n; i++)
            {
                if (data[i] == current && length < 255)
                {
                    length++;
                }
                else
                {
                    compressed.Add(current);
                    compressed.Add(length);
                    current = data[i];
                    length = 1;
                }
            }

            compressed.Add(current);
            compressed.Add(length);

            return compressed;
        }

        // Decompress using RLE
        static byte[] DecompressRLE(List<byte> compressedData)
        {
            List<byte> decompressed = new List<byte>();

            for (int i = 0; i < compressedData.Count; i += 2)
            {
                byte value = compressedData[i];
                byte length = compressedData[i + 1];
                for (int j = 0; j < length; j++)
                {
                    decompressed.Add(value);
                }
            }

            return decompressed.ToArray();
        }
    }
}
