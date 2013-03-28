#region - Using Statements -

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Compression
{
    /// <summary>
    /// Static class that compresses/decompresses files.
    /// </summary>
    public static class Compressor
    {

        #region - Constants -

        private const string DEFAULT_FILE_EXTENSION = ".gz";

        #endregion

        #region - Compression Methods -

        /// <summary>
        /// Compresses the specified original file.
        /// </summary>
        /// <param name="originalFile">The original file.</param>
        /// <param name="compressedFile">The compressed file.</param>
        public static void CompressFile(FileInfo originalFile, FileInfo compressedFile)
        {
            using (FileStream origFileStream = originalFile.OpenRead())
            {
                using (FileStream compressedFileStream = compressedFile.Create())
                {
                    using (GZipStream gzip = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        origFileStream.CopyTo(gzip);
                    }
                }
            }
        }

        /// <summary>
        /// Compresses the specified original file path.
        /// </summary>
        /// <param name="originalFilePath">The original file path.</param>
        /// <param name="compressedFilePath">The compressed file path.</param>
        public static void CompressFile(string originalFilePath, string compressedFilePath)
        {
            FileInfo originalFile = new FileInfo(originalFilePath);
            FileInfo compressedFile = new FileInfo(compressedFilePath);
            CompressFile(originalFile, compressedFile);
        }

        /// <summary>
        /// Compresses the specified original file.
        /// </summary>
        /// <param name="originalFile">The original file.</param>
        public static void CompressFile(FileInfo originalFile)
        {
            FileInfo compressedFile = GetDefaultCompressedFileInfo(originalFile);
            CompressFile(originalFile, compressedFile);
        }

        /// <summary>
        /// Compresses the specified original file path.
        /// </summary>
        /// <param name="originalFilePath">The original file path.</param>
        public static void CompressFile(string originalFilePath)
        {
            FileInfo originalFile = new FileInfo(originalFilePath);
            CompressFile(originalFile);
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">input</exception>
        public static string CompressString(string input, Encoding encoding)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            string output = null;

            using (MemoryStream decomStream = new MemoryStream(encoding.GetBytes(input)))
            {
                using (MemoryStream comStream = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(comStream, CompressionMode.Compress))
                    {
                        decomStream.CopyTo(gzip);
                        decomStream.Flush();
                    }

                    byte[] comBytes = comStream.ToArray();
                    output = Convert.ToBase64String(comBytes);
                }
            }

            return output;
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string CompressString(string input)
        {
            return CompressString(input, Encoding.UTF8);
        }

        #endregion

        #region - Decompression Methods -

        /// <summary>
        /// Decompresses the specified compressed file.
        /// </summary>
        /// <param name="compressedFile">The compressed file.</param>
        /// <param name="decompressedFile">The decompressed file.</param>
        public static void DecompressFile(FileInfo compressedFile, FileInfo decompressedFile)
        {
            using (FileStream comFileStream = compressedFile.OpenRead())
            {
                using (FileStream decomFileStream = decompressedFile.Create())
                {
                    using (GZipStream gzip = new GZipStream(comFileStream, CompressionMode.Decompress))
                    {
                        gzip.CopyTo(decomFileStream);
                    }
                }
            }
        }

        /// <summary>
        /// Decompresses the specified compressed file path.
        /// </summary>
        /// <param name="compressedFilePath">The compressed file path.</param>
        /// <param name="decompressedFilePath">The decompressed file path.</param>
        public static void DecompressFile(string compressedFilePath, string decompressedFilePath)
        {
            FileInfo compressedFile = new FileInfo(compressedFilePath);
            FileInfo decompressedFile = new FileInfo(decompressedFilePath);
            DecompressFile(compressedFile, decompressedFile);
        }

        /// <summary>
        /// Decompresses the specified compressed file.
        /// </summary>
        /// <param name="compressedFile">The compressed file.</param>
        public static void DecompressFile(FileInfo compressedFile)
        {
            FileInfo decompressedFile = GetDefaultDecompressedFileInfo(compressedFile);
            DecompressFile(compressedFile, decompressedFile);
        }

        /// <summary>
        /// Decompresses the specified compressed file path.
        /// </summary>
        /// <param name="compressedFilePath">The compressed file path.</param>
        public static void DecompressFile(string compressedFilePath)
        {
            FileInfo compressedFile = new FileInfo(compressedFilePath);
            DecompressFile(compressedFile);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">input</exception>
        public static string DecompressString(string input, Encoding encoding)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            string output = null;

            using (MemoryStream comStream = new MemoryStream(Convert.FromBase64String(input)))
            {
                using (MemoryStream decomStream = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(comStream, CompressionMode.Decompress))
                    {
                        gzip.CopyTo(decomStream);
                        gzip.Flush();
                    }

                    byte[] decomBytes = decomStream.ToArray();
                    output = encoding.GetString(decomBytes);
                }
            }

            return output;
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string DecompressString(string input)
        {
            return DecompressString(input, Encoding.UTF8);
        }

        #endregion

        #region - Private Methods -

        /// <summary>
        /// Gets the default compressed file info.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private static FileInfo GetDefaultCompressedFileInfo(string filePath)
        {
            if (Path.GetExtension(filePath).Equals(DEFAULT_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
            {
                return new FileInfo(filePath);
            }

            return new FileInfo(filePath + DEFAULT_FILE_EXTENSION);
        }

        /// <summary>
        /// Gets the default compressed file info.
        /// </summary>
        /// <param name="originalFileInfo">The original file info.</param>
        /// <returns></returns>
        private static FileInfo GetDefaultCompressedFileInfo(FileInfo originalFileInfo)
        {
            return GetDefaultCompressedFileInfo(originalFileInfo.FullName);
        }

        /// <summary>
        /// Gets the default decompressed file info.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private static FileInfo GetDefaultDecompressedFileInfo(string filePath)
        {
            if (Path.GetExtension(filePath).Equals(DEFAULT_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
            {
                string path = Path.GetFileNameWithoutExtension(filePath);
                return new FileInfo(path);
            }

            return new FileInfo(filePath);
        }

        /// <summary>
        /// Gets the default decompressed file info.
        /// </summary>
        /// <param name="originalFileInfo">The original file info.</param>
        /// <returns></returns>
        private static FileInfo GetDefaultDecompressedFileInfo(FileInfo originalFileInfo)
        {
            return GetDefaultDecompressedFileInfo(originalFileInfo.FullName);
        }

        #endregion

    }
}
