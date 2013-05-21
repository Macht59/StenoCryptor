using StenoCryptor.Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Models
{
    public class ExtractDwmModel
    {
        public EmbedType EmbedType { get; set; }

        public CryptType CryptType { get; set; }
    }
}