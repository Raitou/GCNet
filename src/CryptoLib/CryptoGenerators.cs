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
using System.Security.Cryptography;

namespace GCNet.CryptoLib
{
    /// <summary>
    /// Provides the generation functions used in Grand Chase Networking
    /// </summary>
    public static class CryptoGenerators
    {
        /// <summary>
        /// Generates an encryption IV (initialization vector)
        /// </summary>
        public static byte[] GenerateIV()
        {
            byte[] outputIV = new byte[8];

            // The byte that will fill all the IV
            byte ivByte;

            Random random = new Random();
            ivByte = (byte)random.Next(0x00, 0xFF);

            for (int i = 0; i < outputIV.Length; i++)
            {
                outputIV[i] = ivByte;
            }            
            return outputIV;
        }

        /// <summary>
        /// Generates a key which may be used in the packet encryption or in the HMAC generation
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateKey()
        {
            byte[] outputKey = new byte[8];

            using (RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(outputKey);
            }
            return outputKey;
        }

        /// <summary>
        /// Generates an HMAC hash to the encrypted packet data
        /// </summary>
        /// <param name="data">Packet data (excluding the packet size and the HMAC hash)</param>
        /// <param name="hmacKey">HMAC Key</param>
        internal static byte[] GenerateHmac(byte[] data, byte[] hmacKey)
        {
            using (HMACMD5 hmac = new HMACMD5(hmacKey))
            {
                // Generates the hash, truncates it to 10 bytes (HMAC_SIZE) and returns it
                return Util.ReadBytes(hmac.ComputeHash(data, 0, data.Length),
                    0, CryptoConstants.GC_HMAC_SIZE);
            }
        }
    }
}