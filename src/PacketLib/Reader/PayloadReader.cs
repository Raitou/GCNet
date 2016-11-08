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

using GCNet.PacketLib.Reader;
using GCNet.Util.Endianness;
using System;
using System.Text;

namespace GCNet.PacketLib
{
    /// <summary>
    /// Represents a packet reader for payload data.
    /// </summary>
    public sealed class PayloadReader : ReaderBase
    {
        /// <summary>
        /// Initializes a new instance of PayloadReader using the specified payload data.
        /// </summary>
        /// <param name="payload">The payload data to be read.</param>
        public PayloadReader(byte[] payload) : base(payload) { }

        /// <summary>
        /// Initializes a new instance of PayloadReader using the specified payload data and starting position.
        /// </summary>
        /// <param name="payload">The payload data to be read.</param>
        /// <param name="startingPosition">The index where the reading will begin.</param>
        public PayloadReader(byte[] payload, int startingPosition) : base(payload)
        {
            Position = startingPosition;
        }


        /// <summary>
        /// Reads a byte from the payload data and advances the current position by 1 byte.
        /// </summary>
        /// <returns>The next byte.</returns>
        public byte ReadByte()
        {
            return Data[Position++];
        }

        /// <summary>
        /// Reads a boolean from the payload data and advances the current position by 1 byte.
        /// </summary>
        /// <returns>The next boolean.</returns>
        public bool ReadBool()
        {
            byte readByte = Data[Position++];
            return Convert.ToBoolean(readByte);
        }

        /// <summary>
        /// Reads a 16-bit integer from the payload data and advances the current position by 2 bytes.
        /// </summary>
        /// <returns>The next 16-bit integer.</returns>
        public short ReadInt16()
        {
            short readShort = BigEndian.GetInt16(Data, Position);
            Position += sizeof(short);

            return readShort;
        }

        /// <summary>
        /// Reads a 32-bit integer from the payload data and advances the current position by 4 bytes.
        /// </summary>
        /// <returns>The next 32-bit integer.</returns>
        public int ReadInt32()
        {
            int readInt = BigEndian.GetInt32(Data, Position);
            Position += sizeof(int);

            return readInt;
        }

        /// <summary>
        /// Reads a 64-bit integer from the payload data and advances the current position by 8 bytes.
        /// </summary>
        /// <returns>The next 64-bit integer.</returns>
        public long ReadInt64()
        {
            long readLong = BigEndian.GetInt64(Data, Position);
            Position += sizeof(long);

            return readLong;
        }

        /// <summary>
        /// Reads a string from the payload data and advances the current position by the string length.
        /// </summary>
        /// <param name="length">The string length.</param>
        /// <returns>The next string.</returns>
        public string ReadString(int length)
        {
            return Encoding.ASCII.GetString(ReadBytes(length));
        }

        /// <summary>
        /// Reads an unicode string from the payload data and advances the current position by the string length.
        /// </summary>
        /// <param name="length">The unicode string length.</param>
        /// <returns>The next unicode string.</returns>
        public string ReadUnicodeString(int length)
        {
            return Encoding.Unicode.GetString(ReadBytes(length));
        }
    }
}