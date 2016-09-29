//-----------------------------------------------------------------------
// GCNet - A Grand Chase Networking Library
// Copyright © 2016  SyntaxDev
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//-----------------------------------------------------------------------

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