using GCNet.CryptoLib;
using GCNet.PacketLib.Abstract;
using System;
using System.Text;

namespace GCNet.PacketLib
{
    /// <summary>
    /// Represents an incoming packet
    /// </summary>
    public class InPacket : AbstractPacket
    {
        /// <summary>
        /// The packet ID
        /// </summary>
        public short ID { get; protected set; }

        /// <summary>
        /// Creates a new instance of InPacket. It reads the packet buffer, decrypts and, if needed, decompress it. Then, it stores the resulting data in 'Data' and the packet id in 'ID'
        /// </summary>
        /// <param name="packetBuffer">Packet data the way it was received</param>
        /// <param name="key">Encryption Key</param>
        public InPacket(byte[] packetBuffer, byte[] key)
        {
            data = packetBuffer;
            data = CryptoFunctions.DecryptPacket(data, key);
            
            if (data.Length > 12)
            {
                if (ReadByte(11) == 0x78 && ReadByte(12) == 0x01)
                {
                    data = Compression.UncompressPacket(data);
                }
            }
            ID = ReadInt16(0);
        }

        /// <summary>
        /// Reads a specified byte array from the packet data
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        /// <param name="length">Amount of bytes to be read</param>
        /// <param name="reverse">Reading mode</param>
        public byte[] ReadBytes(int index, int length)
        {
            byte[] output = new byte[length];
            Buffer.BlockCopy(data, index, output, 0, length);

            return output;
        }

        /// <summary>
        /// Reads a byte from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        public byte ReadByte(int index)
        {
            return data[index];
        }

        /// <summary>
        /// Reads a boolean from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        public bool ReadBool(int index)
        {
            return Convert.ToBoolean(data[index]);
        }

        /// <summary>
        /// Reads a short from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        public short ReadInt16(int index)
        {
            byte[] bytesToRead = ReadBytes(index, sizeof(short));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToRead);
            }
            return BitConverter.ToInt16(bytesToRead, 0);
        }

        /// <summary>
        /// Reads a reversed short from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param> 
        public short ReadReversedInt16(int index)
        {
            byte[] bytesToRead = ReadBytes(index, sizeof(short));
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToRead);
            }
            return BitConverter.ToInt16(bytesToRead, 0);
        }

        /// <summary>
        /// Reads an integer from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        public int ReadInt32(int index)
        {
            byte[] bytesToRead = ReadBytes(index, sizeof(int));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToRead);
            }
            return BitConverter.ToInt32(bytesToRead, 0);
        }

        /// <summary>
        /// Reads a reversed integer from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        public int ReadReversedInt32(int index)
        {
            byte[] bytesToRead = ReadBytes(index, sizeof(int));
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToRead);
            }
            return BitConverter.ToInt32(bytesToRead, 0);
        }

        /// <summary>
        /// Reads a long from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        public long ReadInt64(int index)
        {
            byte[] bytesToRead = ReadBytes(index, sizeof(long));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToRead);
            }
            return BitConverter.ToInt64(bytesToRead, 0);
        }

        /// <summary>
        /// Reads a reversed long from the packet data at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param>
        public long ReadReversedInt64(int index)
        {
            byte[] bytesToRead = ReadBytes(index, sizeof(long));
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToRead);
            }
            return BitConverter.ToInt64(bytesToRead, 0);
        }

        /// <summary>
        /// Reads a string from the packet data starting at a specified index
        /// </summary>
        /// <param name="index">Index where the reading begins</param> 
        /// <param name="length">String length</param>
        public string ReadString(int index, int length)
        {
            return Encoding.ASCII.GetString(ReadBytes(index, length)).Replace("\0", string.Empty);            
        }
    }
}