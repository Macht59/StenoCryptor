using StenoCryptor.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Models
{
    public class DwmEmbedModel
    {
        public byte[] Container { get; set; }

        public string Message { get; set; }

        public EmbedType EmbedType { get; set; }

        public CryptType CryptType { get; set; }
    }
}