using StenoCryptor.Commons;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace StenoCryptor.Web.Models
{
    public class DwmEmbedModel
    {
        [Required(ErrorMessageResourceType = typeof(Localization.Models.DwmEmbedModel), ErrorMessageResourceName = "ErrContainerRequired")]
        public string Message { get; set; }

        public EmbedType EmbedType { get; set; }

        public CryptType CryptType { get; set; }
    }
}