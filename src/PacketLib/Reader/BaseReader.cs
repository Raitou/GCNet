//-----------------------------------------------------------------------
// GCLib - A Grand Chase KOM Library
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

using GCNet.Util;

namespace GCNet.PacketLib.Reader
{
    /// <summary>
    /// Represents the base class for the packet readers.
    /// </summary>
    public abstract class BaseReader
    {
        /// <summary>
        /// Gets or sets the current reading position.
        /// </summary>
        public int Position { get; set; } = 0;
        /// <summary>
        /// Gets the current data being read.
        /// </summary>
        protected byte[] Data { get; }


        /// <summary>
        /// The base constructor for the packet readers. Initializes a new instance of a reader using the given data.
        /// </summary>
        /// <param name="data">The packet data to be read.</param>
        protected BaseReader(byte[] data)
        {
            Data = data;
        }


        /// <summary>
        /// Reads the specified number of bytes from 'Data' into a byte array and advances the current position by that number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to be read.</param>
        /// <returns>The read bytes.</returns>
        public byte[] ReadBytes(int count)
        {
            byte[] readBytes = Sequence.ReadBlock(Data, Position, count);
            Position += count;

            return readBytes;            
        }
    }
}