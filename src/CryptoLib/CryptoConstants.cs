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

namespace GCNet.CryptoLib
{
    /// <summary>
    /// Provides the basic cryptographic constants used in Grand Chase Networking
    /// </summary>
    public static class CryptoConstants
    {
        /// <summary>
        /// Default DES encryption key used at the start of the Grand Chase connection
        /// </summary>
        public static readonly byte[] GC_DES_KEY = { 0xC7, 0xD8, 0xC4, 0xBF, 0xB5, 0xE9, 0xC0, 0xFD };

        /// <summary>
        /// Default HMAC MD5 key used at the start of the Grand Chase connection
        /// </summary>
        public static readonly byte[] GC_HMAC_KEY = { 0xC0, 0xD3, 0xBD, 0xC3, 0xB7, 0xCE, 0xB8, 0xB8 };

        /// <summary>
        /// Size of the truncated generated HMAC 
        /// </summary>
        public static readonly byte GC_HMAC_SIZE = 10;
    }
}
