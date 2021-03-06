﻿using StenoCryptor.Commons;
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
    public static class DwmProcessorHelper
    {
        /// <summary>
        /// Embeds DWM within container.
        /// </summary>
        /// <param name="cryptor">Crypt algorithm.</param>
        /// <param name="embeder">Embeding algorithm.</param>
        /// <param name="message">Secret message.</param>
        /// <param name="key">Crypting key.</param>
        /// <param name="container">Container.</param>
        /// <param name="fileName">Container file name.</param>
        /// <returns>Saved file name at server.</returns>
        public static void EmbedDwm(ICryptor cryptor, IEmbeder embeder, string message, Key key, Container container)
        {
            Stream cryptedMessage = cryptor.Encrypt(StreamHelper.StringToStream(message), key);
            embeder.Embed(container, StreamHelper.StreamToBytesArray(cryptedMessage));
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

        public static string ExtractDwm(IEmbeder embeder, ICryptor cryptor, Key key, Container container)
        {
            byte[] byteMessage = embeder.Extract(container, key);
            Stream message = cryptor.Decrypt(StreamHelper.BytesToStream(byteMessage), key);

            return StreamHelper.StreamToString(message);
        }
    }
}
