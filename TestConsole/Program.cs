#region - Using Statements -

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compression;

#endregion

namespace TestConsole
{
    class Program
    {

        private static Lazy<string> _fileToCompressPath = new Lazy<string>(() => ConfigurationManager.AppSettings["FileToCompress"]);

        static void Main(string[] args)
        {
            TestStringCompression();
        }

        static void TestStringCompression()
        {
            Console.Write("Enter a string to compress:  ");
            string original = Console.ReadLine();
            string compressed = Compressor.CompressString(original);
            Console.WriteLine(compressed);
            string decompressed = Compressor.DecompressString(compressed);
            Console.WriteLine(decompressed);

            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }

        static void TestFileCompression()
        {
            FileInfo originalFile = GetFileToCompress();
            FileInfo compressedFile = GetCompressedFile();
            FileInfo decompressedFile = GetDecompressedFile();

            Compressor.CompressFile(originalFile, compressedFile);
            Compressor.DecompressFile(compressedFile, decompressedFile);
        }

        static FileInfo GetFileToCompress()
        {
            return new FileInfo(_fileToCompressPath.Value);
        }

        static FileInfo GetCompressedFile()
        {
            string path = Path.GetDirectoryName(_fileToCompressPath.Value);
            return new FileInfo(Path.Combine(path, "compressed.txt"));
        }

        static FileInfo GetDecompressedFile()
        {
            string path = Path.GetDirectoryName(_fileToCompressPath.Value);
            return new FileInfo(Path.Combine(path, "decompressed.txt"));
        }

    }
}
