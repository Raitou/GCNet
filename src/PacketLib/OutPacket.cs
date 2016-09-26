using GCNet.CryptoLib;
using GCNet.PacketLib.Abstract;
using System;
using System.Text;

namespace GCNet.PacketLib
{
    /// <summary>
    /// Represents an outgoing packet
    /// </summary>
    public class OutPacket : AbstractPacket
    {
        /// <summary>
        /// Creates a new instance of OutPacket, starting by writing the packet id to the data.
        /// <para>* After this, the packet data should be written through the 'Write' methods and, finally, the function 'AssemblePacket' called, returning the packet ready to be sent.</para>
        /// </summary>
        /// <param name="packetID">The outgoing packet ID</param>
        public OutPacket(short packetID)
        {
            data = new byte[0];
            WriteData(packetID);
        }

        /// <summary>
        /// Returns the packet data ready to be sent
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <param name="hmacKey">HMAC generation key</param>
        /// <param name="prefix">The 6 bytes between the packet size and the IV</param>
        public byte[] Assemble(byte[] key, byte[] hmacKey, byte[] prefix)
        {
            byte[] IV = CryptoGenerators.GenerateIV();           
            
            byte[] dataToAssemble = Util.ConcatBytes(
                prefix,
                Util.ConcatBytes(IV, CryptoFunctions.EncryptPacket(data, key, IV)));

            return CryptoFunctions.ClearPacket(dataToAssemble, hmacKey);
        }

        /// <summary>
        /// Compress the data and returns the packet ready to be sent
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <param name="hmacKey">HMAC generation key</param>
        /// <param name="prefix">The 6 bytes between the packet size and the IV</param>
        public byte[] CompressAndAssemble(byte[] key, byte[] hmacKey, byte[] prefix)
        {
            byte[] compressedData = Compression.CompressPacket(data);
            byte[] IV = CryptoGenerators.GenerateIV();

            byte[] dataToAssemble = Util.ConcatBytes(
                prefix,
                Util.ConcatBytes(IV, CryptoFunctions.EncryptPacket(compressedData, key, IV)));

            return CryptoFunctions.ClearPacket(dataToAssemble, hmacKey);
        }

        #region Write Data
        /// <summary>
        /// Writes a specified byte array to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be written</param>
        public void WriteData(byte[] valueToWrite)
        {
            WriteBytes(valueToWrite);
        }

        /// <summary>
        /// Writes a specified byte to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be written</param>
        public void WriteData(byte valueToWrite)
        {
            WriteBytes(new byte[] { valueToWrite });
        }

        /// <summary>
        /// Writes a specified boolean to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be written</param>
        public void WriteData(bool valueToWrite)
        {
            WriteBytes(new byte[] { Convert.ToByte(valueToWrite) });
        }

        /// <summary>
        /// Writes a specified short to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be written</param>
        public void WriteData(short valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);
        }

        /// <summary>
        /// Writes a specified integer to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be written</param>
        public void WriteData(int valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);
        }

        /// <summary>
        /// Writes a specified long to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be written</param>
        public void WriteData(long valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);
        }

        /// <summary>
        /// Writes a specified string to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be written</param>
        public void WriteData(string valueToWrite)
        {
            string outputString = "";

            for (int i = 0; i < valueToWrite.Length; i++)
            {
                outputString += valueToWrite[i] + "\0";
            }

            WriteBytes(Encoding.ASCII.GetBytes(outputString));
        }
        #endregion

        #region Write Reversed Data
        /// <summary>
        /// Writes the reversed specified byte array to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(byte[] valueToWrite)
        {
            Array.Reverse(valueToWrite);
            WriteBytes(valueToWrite);
        }

        /// <summary>
        /// Writes the reversed specified short to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(short valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);
        }

        /// <summary>
        /// Writes the reversed specified integer to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(int valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);

            Console.WriteLine(BitConverter.ToString(bytesToWrite).Replace('-', ' '));
        }

        /// <summary>
        /// Writes the reversed specified long to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(long valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);
        }
        #endregion

        /// <summary>
        /// Writes the specified bytes to the packet data
        /// </summary>
        /// <param name="bytes">Bytes to be written</param>
        protected void WriteBytes(byte[] bytes)
        {
            data = Util.ConcatBytes(data, bytes);
        }
    }
}