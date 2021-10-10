using System.IO;
using System.IO.Compression;
using System.Text;

namespace SharpFunction.API
{
    /// <summary>
    /// Utilities for working with bytes
    /// </summary>
    public static class ByteUtils
    {
        /// <summary>
        ///     Zips string into bytes
        /// </summary>
        /// <param name="str">String to be compressed</param>
        /// <returns>Compressed string</returns>
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }

            return mso.ToArray();
        }

        /// <summary>
        ///     Decompresses the bytes into string
        /// </summary>
        /// <param name="bytes">Bytes to be decompressed</param>
        /// <returns>Decompressed string</returns>
        public static string Unzip(byte[] bytes)
        {
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                gs.CopyTo(mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
}