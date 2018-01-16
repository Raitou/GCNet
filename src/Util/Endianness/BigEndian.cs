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

namespace GCNet.Util.Endianness
{
    internal static class BigEndian
    {
        public static short GetInt16(byte[] bytes, int index)
        {
            return (short)
                ((bytes[index] << 8) | (bytes[index + 1]));
        }

        public static int GetInt32(byte[] bytes, int index)
        {
            return (bytes[index] << 24) |
                   (bytes[index + 1] << 16) |
                   (bytes[index + 2] << 8) |
                   (bytes[index + 3]);
        }

        public static long GetInt64(byte[] bytes, int index)
        {
            return (bytes[index] << 56) |
                   (bytes[index + 1] << 48) |
                   (bytes[index + 2] << 40) |
                   (bytes[index + 3] << 32) |
                   (bytes[index + 4] << 24) |
                   (bytes[index + 5] << 16) |
                   (bytes[index + 6] << 8) |
                   (bytes[index + 7]);
        }

        public static byte[] GetBytes(short int16)
        {
            byte[] bytes = new byte[sizeof(short)];

            bytes[0] = (byte)(int16 >> 8);
            bytes[1] = (byte)(int16);

            return bytes;
        }

        public static byte[] GetBytes(int int32)
        {
            byte[] bytes = new byte[sizeof(int)];

            bytes[0] = (byte)(int32 >> 24);
            bytes[1] = (byte)(int32 >> 16);
            bytes[2] = (byte)(int32 >> 8);
            bytes[3] = (byte)(int32);

            return bytes;
        }

        public static byte[] GetBytes(long int64)
        {
            byte[] bytes = new byte[sizeof(long)];

            bytes[0] = (byte)(int64 >> 56);
            bytes[1] = (byte)(int64 >> 48);
            bytes[2] = (byte)(int64 >> 40);
            bytes[3] = (byte)(int64 >> 32);
            bytes[4] = (byte)(int64 >> 24);
            bytes[5] = (byte)(int64 >> 16);
            bytes[6] = (byte)(int64 >> 8);
            bytes[7] = (byte)(int64);

            return bytes;
        }
    }
}