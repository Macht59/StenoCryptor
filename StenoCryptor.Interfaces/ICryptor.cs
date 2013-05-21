using StenoCryptor.Commons;
using System.IO;

namespace StenoCryptor.Interfaces
{
    public interface ICryptor: IKeyAware
    {
        /// <summary>
        /// Encrypts stream.
        /// </summary>
        /// <param name="message">Data to encrypt.</param>
        /// <param name="key">Encription key.</param>
        /// <returns>Encrypted data.</returns>
        Stream Encrypt(Stream message, Key key);

        /// <summary>
        /// Decrypts stream.
        /// </summary>
        /// <param name="message">Data to decrypt.</param>
        /// <param name="key">Encryption key.</param>
        /// <returns>Dectypted data.</returns>
        Stream Decrypt(Stream message, Key key);
    }
}
