using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Commons.Enums;
using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StenoCryptor.Desktop.Helpers
{
    public static class KeyMakerHelper
    {
        public static Key GenerateKey(Container container, string message, CryptType cryptType, EmbedType embedType, IKeyAware keyParser, string stringkey)
        {
            if (!keyParser.ValidateKey(stringkey))
                throw new ArgumentException("Key is not valid.");

            Key key = new Key();
            key.CryptType = cryptType;
            key.EmbedType = embedType;
            key.MessageLength = Constants.DEFAULT_ENCODING.GetByteCount(message);
            key.Value = keyParser.ParseKey(stringkey);    

            return key;
        }
    }
}