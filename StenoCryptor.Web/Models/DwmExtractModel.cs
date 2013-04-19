using StenoCryptor.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Models
{
    public class DwmExtractModel
    {
        public Stream Container { get; set; }

        public string Message { get; set; }

        public EmbedType EmbedType { get; set; }

        public CryptType CryptType { get; set; }
    }
}