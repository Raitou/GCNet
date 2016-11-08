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

namespace GCNet.PacketLib
{
    /// <summary>
    /// Represents an incoming packet.
    /// </summary>
    public class InPacket
    {
        /// <summary>
        /// Gets the processed payload data of the current packet.
        /// </summary>
        public byte[] Payload { get; protected set; }

        /// <summary>
        /// Gets the size of the current packet.
        /// </summary>
        public short Size { get; private set; }
        /// <summary>
        /// Gets the prefix of the current packet.
        /// </summary>
        public short Prefix { get; private set; }
        /// <summary>
        /// Gets the count in the current packet header.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the current packet ID.
        /// </summary>
        public short Id { get; }


        /// <summary>
        /// Initializes a new instance of InPacket from the given packet buffer and crypto handler.
        /// </summary>
        /// <param name="packetBuffer">The packet buffer the way it was received.</param>
        /// <param name="crypto">The current crypto handler.</param>
        public InPacket(byte[] packetBuffer, CryptoHandler crypto)
        {
            ParseHeader(packetBuffer);
            Payload = crypto.DecryptPacket(packetBuffer);
            
            PayloadReader reader = new PayloadReader(Payload);
            Id = reader.ReadInt16();
        }

        /// <summary>
        /// Parses the header of the current packet and assigns the read values to the proper variables.
        /// </summary>
        /// <param name="packetBuffer">The packet buffer the way it was received.</param>
        private void ParseHeader(byte[] packetBuffer)
        {
            HeaderReader reader = new HeaderReader(packetBuffer);

            Size = reader.ReadInt16();
            Prefix = reader.ReadInt16();
            Count = reader.ReadInt32();
        }
    }
}