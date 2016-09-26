using Ionic.Zlib;
using System.IO;

namespace GCNet.PacketLib
{
    /// <summary>
    /// Provides the data compression functions which will be used in the Grand Chase networking
    /// </summary>
    static class Compression
    {
        /// <summary>
        /// Returns the compressed packet from the input data
        /// </summary>
        /// <param name="dataToCompress">Packet data to be compressed</param>
        public static byte[] CompressPacket(byte[] dataToCompress)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZlibStream compressor =
                    new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.Level1))
                {
                    compressor.Write(dataToCompress, 11, (dataToCompress.Length - 11));
                }
                return Util.ConcatBytes(Util.ReadBytes(dataToCompress, 0, 11), memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Returns the uncompressed packet from the input data
        /// </summary>
        /// <param name="packetToUncompress">Packet data to be uncompressed</param>
        /// <returns></returns>
        public static byte[] UncompressPacket(byte[] packetToUncompress)
        {
            return Util.ConcatBytes(
                Util.ReadBytes(packetToUncompress, 0, 11),
                ZlibStream.UncompressBuffer(Util.ReadBytes(packetToUncompress, 11, (packetToUncompress.Length - 11))));
        }
    }
}