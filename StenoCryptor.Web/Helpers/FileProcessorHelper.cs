using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Interfaces;
using StenoCryptor.Web.Models;
using System;
using System.IO;

namespace StenoCryptor.Web.Helpers
{
    /// <summary>
    /// Uses engine to process files and DWM.
    /// </summary>
    public static class FileProcessorHelper
    {
        /// <summary>
        /// Embeds DWM within container file.
        /// </summary>
        /// <param name="cryptor">Crypt algorithm.</param>
        /// <param name="embeder">Embeding algorithm.</param>
        /// <param name="message">Secret message.</param>
        /// <param name="key">Crypting key.</param>
        /// <param name="container">Container.</param>
        /// <param name="fileName">Container file name.</param>
        /// <returns>Saved file name at server.</returns>
        public static string EmbedDwm(ICryptor cryptor, IEmbeder embeder, string message, Key key, Container container, string fileName)
        {
            if (!cryptor.ValidateKey(key, container))
                throw new ArgumentException(Localization.Views.Shared.WrongKey);
            
            //Add message length to the DWM
            int messageLength = Constants.DEFAULT_ENCODING.GetByteCount(message);
            MemoryStream messageLengthStream = new MemoryStream(BitConverter.GetBytes(messageLength));

            //Ctypting the message
            Stream cryptedMessage = cryptor.Encrypt(StreamHelper.StringToStream(message), key);

            //Concat length and message into single DWM
            cryptedMessage = StreamHelper.AppendToStream(messageLengthStream, cryptedMessage);
            
            //Embeding DWM
            embeder.Embed(container, StreamHelper.StreamToBytesArray(cryptedMessage));

            return StreamHelper.SaveFile(container.InputStream, fileName);
        }

        /// <summary>
        /// Detects is there Dwm in container.
        /// </summary>
        /// <param name="detector">Detecting algorithm.</param>
        /// <param name="inputStream">Conteiner file stream.</param>
        /// <returns>True if Dwm exists in container or false if it is not.</returns>
        public static bool DetectDwm(IDetector detector, Stream inputStream)
        {
            Container container = new Container(inputStream);
            return detector.Detect(container);
        }

        public static DwmModel ExtractDwm(Container container, IEmbeder embeder)
        {
            
            return new DwmModel();
        }
    }
}
