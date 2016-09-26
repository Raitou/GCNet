namespace GCNet.PacketLib.Abstract
{
    /// <summary>
    /// Base model for the Grand Chase packets
    /// </summary>
    public class AbstractPacket
    {
        /// <summary>
        /// The raw packet data (uncompressed and not encrypted)
        /// </summary>
        public byte[] data { get; protected set; }
    }
}
