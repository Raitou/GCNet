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

using GCNet.CoreLib;
using GCNet.Util;
using GCNet.Util.Endianness;
using System.Text;

namespace GCNet.PacketLib
{
    /// <summary>
    /// Represents a packet reader for payload data.
    /// </summary>
    public sealed class PayloadReader
    {
        /// <summary>
        /// Gets the operation code (opcode) of the current payload's packet.
        /// </summary>
        public short Opcode
        {
            get { return BigEndian.GetInt16(_payloadData, OPCODE_OFFSET); }
        }

        /// <summary>
        /// Gets the length of the current payload's content in bytes.
        /// </summary>
        public int ContentLength
        {
            get { return BigEndian.GetInt32(_payloadData, CONTENT_LENGTH_OFFSET); }
        }

        /// <summary>
        /// Gets the value of the current payload's compression flag.
        /// </summary>
        public bool CompressionFlag
        {
            get { return _payloadData[COMPRESSION_FLAG_OFFSET] == 1; }
        }

        private const int OPCODE_OFFSET = 0;
        private const int CONTENT_LENGTH_OFFSET = 2;
        private const int COMPRESSION_FLAG_OFFSET = 6;

        private byte[] _payloadData;
        private int _position = 7;

        /// <summary>
        /// Initializes a new instance of PayloadReader using the specified payload data. If the
        /// payload is compressed, it is automatically decompressed.
        /// </summary>
        /// <param name="payload">The payload data to be read.</param>
        /// <remarks>The null bytes padding is ignored by the reader.</remarks>
        public PayloadReader(byte[] payload)
        {
            _payloadData = payload;

            if (CompressionFlag)
            {
                byte[] firstPart = Sequence.ReadBlock(_payloadData, 0, 11);
                byte[] decompressedContent = ZLib.DecompressData(Sequence.ReadBlock(_payloadData, 11, ContentLength - sizeof(int)));
                byte[] zeroesPadding = new byte[4];

                _payloadData = Sequence.Concat(firstPart, decompressedContent, zeroesPadding);
            }
        }

        /// <summary>
        /// Skips a specified amount of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to be skipped.</param>
        public void Skip(int count)
        {
            _position += count;
        }

        /// <summary>
        /// Reads a byte from the payload's content and advances the current position by 1 byte.
        /// </summary>
        /// <returns>The next byte.</returns>
        public byte ReadByte()
        {
            return _payloadData[_position++];
        }

        /// <summary>
        /// Reads a boolean from the payload's content and advances the current position by 1 byte.
        /// </summary>
        /// <returns>The next boolean.</returns>
        public bool ReadBool()
        {
            return ReadByte() == 1;
        }

        /// <summary>
        /// Reads a 16-bit integer from the payload's content and advances the current position by 2 bytes.
        /// </summary>
        /// <returns>The next 16-bit integer.</returns>
        public short ReadInt16()
        {
            short int16 = BigEndian.GetInt16(_payloadData, _position);
            _position += sizeof(short);

            return int16;
        }

        /// <summary>
        /// Reads a 32-bit integer from the payload's content and advances the current position by 4 bytes.
        /// </summary>
        /// <returns>The next 32-bit integer.</returns>
        public int ReadInt32()
        {
            int int32 = BigEndian.GetInt32(_payloadData, _position);
            _position += sizeof(int);

            return int32;
        }

        /// <summary>
        /// Reads a 64-bit integer from the payload's content and advances the current position by 8 bytes.
        /// </summary>
        /// <returns>The next 64-bit integer.</returns>
        public long ReadInt64()
        {
            long int64 = BigEndian.GetInt64(_payloadData, _position);
            _position += sizeof(long);

            return int64;
        }

        /// <summary>
        /// Reads a string from the payload's content and advances the current position by the string length.
        /// </summary>
        /// <param name="length">The string length.</param>
        /// <returns>The next string.</returns>
        public string ReadString(int length)
        {
            return Encoding.ASCII.GetString(ReadBytes(length));
        }

        /// <summary>
        /// Reads an unicode string from the payload's content and advances the current position by
        /// the string length.
        /// </summary>
        /// <param name="length">The unicode string length.</param>
        /// <returns>The next unicode string.</returns>
        public string ReadUnicodeString(int length)
        {
            return Encoding.Unicode.GetString(ReadBytes(length));
        }

        /// <summary>
        /// Reads the specified number of bytes from 'Data' into a byte array and advances the
        /// current position by that number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to be read.</param>
        /// <returns>The read bytes.</returns>
        public byte[] ReadBytes(int count)
        {
            byte[] data = Sequence.ReadBlock(_payloadData, _position, count);
            _position += count;

            return data;
        }
    }
}