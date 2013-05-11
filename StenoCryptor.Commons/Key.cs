using System;

namespace StenoCryptor.Commons
{
    /// <summary>
    /// Key object for StenoCtyptor.
    /// </summary>
    [Serializable]
    public class Key
    {
        /// <summary>
        /// Length of hidden message.
        /// </summary>
        public long MessageLength { get; set; }

        /// <summary>
        /// Key itself.
        /// </summary>
        public byte[] Value { get; set; }
    }
}
