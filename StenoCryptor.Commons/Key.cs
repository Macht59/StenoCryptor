using StenoCryptor.Commons.Enums;
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
        public int MessageLength { get; set; }

        /// <summary>
        /// Crypt type this key used for.
        /// </summary>
        public CryptType CryptType { get; set; }

        /// <summary>
        /// Embeding type.
        /// </summary>
        public EmbedType EmbedType { get; set; }

        /// <summary>
        /// Key itself.
        /// </summary>
        public byte[] Value { get; set; }
    }
}
