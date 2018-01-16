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
using System.Linq;
using System.Security.Cryptography;

namespace GCNet.CoreLib
{
    /// <summary>
    /// Represents a packet authentication handler.
    /// </summary>
    public sealed class AuthHandler
    {
        private const int HMAC_LENGTH = 10;
        private const int AUTH_DATA_OFFSET = 2;

        private byte[] _hmacKey;

        /// <summary>
        /// Initializes a new instance of the AuthHandler class using the default HMAC key.
        /// </summary>
        public AuthHandler()
        {
            _hmacKey = new byte[] { 0xC0, 0xD3, 0xBD, 0xC3, 0xB7, 0xCE, 0xB8, 0xB8 };
        }

        /// <summary>
        /// Initializes a new instance of the AuthHandler class using the given HMAC key.
        /// </summary>
        /// <param name="hmacKey">
        /// The HMAC key which will be used by the new instance of the authentication handler.
        /// </param>
        public AuthHandler(byte[] hmacKey)
        {
            _hmacKey = hmacKey;
        }

        /// <summary>
        /// Computes the HMAC for the specified packet data.
        /// </summary>
        /// <param name="authData">The whole packet buffer, except for the size and the own HMAC.</param>
        /// <returns>The HMAC of the packet, which has the size of 10 bytes.</returns>
        public byte[] GetHmac(byte[] authData)
        {
            using (var hmac = new HMACMD5(_hmacKey))
            {
                byte[] fullHmac = hmac.ComputeHash(authData);
                return Sequence.ReadBlock(fullHmac, 0, HMAC_LENGTH);
            }
        }

        /// <summary>
        /// Checks the validity of the stored HMAC in the packet buffer.
        /// </summary>
        /// <param name="packetData">The packet the way it was received.</param>
        /// <returns>A boolean that indicates whether the stored HMAC is valid or not.</returns>
        public bool VerifyHmac(byte[] packetData)
        {
            byte[] storedHmac = Sequence.ReadBlock(packetData, packetData.Length - HMAC_LENGTH, HMAC_LENGTH);

            byte[] authData = Sequence.ReadBlock(packetData, AUTH_DATA_OFFSET, packetData.Length - HMAC_LENGTH - AUTH_DATA_OFFSET);
            byte[] expectedHmac = GetHmac(authData);

            return storedHmac.SequenceEqual(expectedHmac);
        }
    }
}