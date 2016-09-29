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

using System;

namespace GCNet
{
    /// <summary>
    /// Provides some utilities
    /// </summary>
    static class Util
    {
        /// <summary>
        /// Returns a specified block of bytes from the source array starting at a defined offset
        /// </summary>
        /// <param name="source">Source byte array from where the block will be read</param>
        /// <param name="offset">Index from where the reading will begin</param>
        /// <param name="length">Number of bytes that will be read</param>
        public static byte[] ReadBytes(byte[] source, int offset, int length)
        {
            byte[] outputBytes = new byte[length];
            Buffer.BlockCopy(source, offset, outputBytes, 0, length);

            return outputBytes;
        }

        /// <summary>
        /// Concatenates two array of bytes
        /// </summary>
        /// <param name="firstBytes">The first byte array to be concatenated</param>
        /// <param name="secondBytes">The second byte array to be concatenated</param>
        /// <returns></returns>
        public static byte[] ConcatBytes(byte[] firstBytes, byte[] secondBytes)
        {
            byte[] outputBytes = new byte[firstBytes.Length + secondBytes.Length];
            
            Buffer.BlockCopy(firstBytes, 0, outputBytes, 0, firstBytes.Length);
            Buffer.BlockCopy(secondBytes, 0, outputBytes, firstBytes.Length, secondBytes.Length);

            return outputBytes;
        }
    }
}
