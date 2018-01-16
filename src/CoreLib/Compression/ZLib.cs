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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//-----------------------------------------------------------------------

using Ionic.Zlib;
using System.IO;

namespace GCNet.CoreLib
{
    internal static class ZLib
    {
        public static byte[] CompressData(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Level1))
                {
                    compressor.Write(data, 0, data.Length);
                    return ms.ToArray();
                }
            }
        }

        public static byte[] DecompressData(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }
    }
}