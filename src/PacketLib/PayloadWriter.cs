﻿//-----------------------------------------------------------------------
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
using System;
using System.Text;

namespace GCNet.PacketLib
{
    /// <summary>
    /// Represents a packet writer for payload data.
    /// </summary>
    public sealed class PayloadWriter
    {
        private byte[] _contentData = new byte[0];

        /// <summary>
        /// Builds a payload from the current content's data.
        /// </summary>
        /// <param name="opcode">The packet's opcode.</param>
        /// <returns>A new payload.</returns>
        public byte[] GetPayload(short opcode)
        {
            byte[] packetId = BigEndian.GetBytes(opcode);
            byte[] size = BigEndian.GetBytes(_contentData.Length);
            byte[] compressionFlag = { 0 }; // false
            byte[] padding = { 0, 0, 0, 0 };

            return Sequence.Concat(packetId, size, compressionFlag, _contentData, padding);
        }

        /// <summary>
        /// Builds a compressed payload from the current content's data.
        /// </summary>
        /// <param name="opcode">The packet's opcode.</param>
        /// <returns>A new compressed payload.</returns>
        public byte[] GetCompressedPayload(short opcode)
        {
            byte[] compressedData = ZLib.CompressData(_contentData);

            byte[] packetId = BigEndian.GetBytes(opcode);
            byte[] size = BigEndian.GetBytes(compressedData.Length + sizeof(int));
            byte[] compressionFlag = { 1 }; // true
            byte[] decompressedSize = LittleEndian.GetBytes(_contentData.Length);
            byte[] padding = { 0, 0, 0, 0 };

            return Sequence.Concat(packetId, size, compressionFlag, decompressedSize, compressedData, padding);
        }

        /// <summary>
        /// Writes the specified byte to the current payload's content.
        /// </summary>
        /// <param name="value">The byte to be written.</param>
        public void WriteData(byte value)
        {
            WriteData(new byte[] { value });
        }

        /// <summary>
        /// Writes the specified boolean to the current payload's content.
        /// </summary>
        /// <param name="boolean">The boolean to be written.</param>
        public void WriteData(bool boolean)
        {
            WriteData(new byte[] { Convert.ToByte(boolean) });
        }

        /// <summary>
        /// Writes the specified 16-bit integer to the current payload's content.
        /// </summary>
        /// <param name="int16">The 16-bit integer to be written.</param>
        public void WriteData(short int16)
        {
            WriteData(BigEndian.GetBytes(int16));
        }

        /// <summary>
        /// Writes the specified 32-bit integer to the current payload's content.
        /// </summary>
        /// <param name="int32">The 32-bit integer to be written.</param>
        public void WriteData(int int32)
        {
            WriteData(BigEndian.GetBytes(int32));
        }

        /// <summary>
        /// Writes the specified 64-bit integer to the current payload's content.
        /// </summary>
        /// <param name="int64">The 64-bit integer to be written.</param>
        public void WriteData(long int64)
        {
            WriteData(BigEndian.GetBytes(int64));
        }

        /// <summary>
        /// Writes the specified string to the current payload's content.
        /// </summary>
        /// <param name="str">The string to be written.</param>
        public void WriteData(string str)
        {
            WriteData(Encoding.ASCII.GetBytes(str));
        }

        /// <summary>
        /// Writes the specified unicode string to the current payload's content.
        /// </summary>
        /// <param name="ustr">The unicode string to be written.</param>
        public void WriteUnicodeString(string ustr)
        {
            WriteData(Encoding.Unicode.GetBytes(ustr));
        }

        /// <summary>
        /// Writes the specified bytes to the current content's data.
        /// </summary>
        /// <param name="bytes">The bytes to be written.</param>
        public void WriteData(byte[] bytes)
        {
            _contentData = Sequence.Concat(_contentData, bytes);
        }
    }
}