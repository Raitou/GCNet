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

using GCNet.Util;

namespace GCNet.CoreLib
{
    /// <summary>
    /// Represents a payload compression handler.
    /// </summary>
    public class CompressionHandler
    {
        /// <summary>
        /// Gets the compression flag value of the current handler's packet.
        /// <para>
        /// * When the CompressionFlag is true, it means the payload is compressed (or will be compressed). When it's false, it means the payload is uncompressed and will remain this way.
        /// </para>
        /// </summary>
        public bool CompressionFlag
        {
            get { return (PayloadData[6] == 0x01); }                        
        }

        /// <summary>
        /// Gets the current handler's payload data.
        /// </summary>
        private byte[] PayloadData { get; }


        /// <summary>
        /// Initializes a new instance of CompressionHandler using the given payload data.
        /// </summary>
        /// <param name="payload">The payload data which will be handled by the new instance of CompressionHandler.</param>
        /// <remarks>
        /// The payload must have the '00 00 00' padding.
        /// </remarks>
        public CompressionHandler(byte[] payload)
        {
            PayloadData = payload;
        }

        
        /// <summary>
        /// Gets the the compressed payload data of the current handler.
        /// </summary>
        /// <returns>The compressed payload data.</returns>
        public byte[] GetCompressedPayload()
        {
            byte[] firstPart = Sequence.ReadBlock(PayloadData, 0, 11);
            byte[] uncompressedData = Sequence.ReadBlock(PayloadData, 11, PayloadData.Length - 11 - 3);
            byte[] nullBytesPadding = new byte[3];

            byte[] compressedData = ZLib.CompressData(uncompressedData);
            
            return Sequence.Concat(firstPart, compressedData, nullBytesPadding);
        }

        /// <summary>
        /// Gets the decompressed payload data of the current handler.
        /// </summary>
        /// <returns>The decompressed payload data.</returns>
        public byte[] GetDecompressedPayload()
        {
            byte[] firstPart = Sequence.ReadBlock(PayloadData, 0, 11);
            byte[] compressedData = Sequence.ReadBlock(PayloadData, 11, PayloadData.Length - 11 - 3);
            byte[] nullBytesPadding = new byte[3];

            byte[] decompressedData = ZLib.DecompressData(compressedData);

            return Sequence.Concat(firstPart, decompressedData, nullBytesPadding);
        }        
    }
}