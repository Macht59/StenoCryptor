using StenoCryptor.Commons;
using StenoCryptor.Commons.Enums;
using System.IO;

namespace StenoCryptor.Web.Models
{
    public class DwmDetectModel
    {
        public Stream Container { get; set; }

        public string Message { get; set; }

        public EmbedType EmbedType { get; set; }

        public CryptType CryptType { get; set; }
    }
}