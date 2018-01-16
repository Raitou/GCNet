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
    /// Represents a packet encryption handler.
    /// </summary>
    public sealed class CryptoHandler
    {
        private byte[] _key;

        /// <summary>
        /// Initializes a new instance of the CryptoHandler class using the default encryption key.
        /// </summary>
        public CryptoHandler()
        {
            _key = new byte[] { 0xC7, 0xD8, 0xC4, 0xBF, 0xB5, 0xE9, 0xC0, 0xFD };
        }

        /// <summary>
        /// Initializes a new instance of the CryptoHandler class using the given encryption key.
        /// </summary>
        /// <param name="key">The encryption key which will be used by the crypto handler.</param>
        public CryptoHandler(byte[] key)
        {
            _key = key;
        }

        /// <summary>
        /// Encrypts the given packet payload data.
        /// </summary>
        /// <param name="payload">The payload data to be encrypted.</param>
        /// <param name="iv">The initialization vector (IV).</param>
        /// <returns>The encrypted payload data.</returns>
        public byte[] EncryptPacket(byte[] payload, byte[] iv)
        {
            byte[] paddedData = PadData(payload);
            return EncryptData(paddedData, iv, _key);
        }

        /// <summary>
        /// Decrypts the specified packet buffer.
        /// </summary>
        /// <param name="packetData">The packet the way it was received.</param>
        /// <returns>The decrypted packet data.</returns>
        public byte[] DecryptPacket(byte[] packetData)
        {
            byte[] iv = Sequence.ReadBlock(packetData, 8, 8);
            byte[] encryptedData = Sequence.ReadBlock(packetData, 16, packetData.Length - 10 - 16);

            byte[] decryptedData = DecryptData(encryptedData, iv, _key);
            int paddingLength = decryptedData.Last() + 2;

            return Sequence.ReadBlock(decryptedData, 0, decryptedData.Length - paddingLength);
        }

        private static byte[] EncryptData(byte[] data, byte[] iv, byte[] key)
        {
            using (var desProvider = new DESCryptoServiceProvider() { Mode = CipherMode.CBC, Padding = PaddingMode.None })
            using (ICryptoTransform encryptor = desProvider.CreateEncryptor(key, iv))
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        private static byte[] DecryptData(byte[] data, byte[] iv, byte[] key)
        {
            using (var desProvider = new DESCryptoServiceProvider() { Mode = CipherMode.CBC, Padding = PaddingMode.None })
            using (ICryptoTransform decryptor = desProvider.CreateDecryptor(key, iv))
            {
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        private byte[] PadData(byte[] data)
        {
            const int ENCRYPTION_BLOCK_LENGTH = 8;

            int distance = ENCRYPTION_BLOCK_LENGTH - data.Length % ENCRYPTION_BLOCK_LENGTH;
            int paddingLength = distance >= 2 ? distance : ENCRYPTION_BLOCK_LENGTH + distance;

            var padding = new byte[paddingLength];

            for (byte i = 1; i < paddingLength; i++)
            {
                padding[i - 1] = i;
            }
            padding[paddingLength - 1] = padding[paddingLength - 2]; // Equals the last to the penultimate byte.

            return Sequence.Concat(data, padding);
        }
    }
}