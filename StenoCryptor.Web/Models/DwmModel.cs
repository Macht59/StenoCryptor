using StenoCryptor.Commons.Enums;
using System.ComponentModel.DataAnnotations;

namespace StenoCryptor.Web.Models
{
    public class DwmModel
    {
        [Required(ErrorMessageResourceType = typeof(Localization.Models.DwmEmbedModel), ErrorMessageResourceName = "ErrContainerRequired")]
        public string Message { get; set; }

        public EmbedType EmbedType { get; set; }

        public CryptType CryptType { get; set; }

        public string Key { get; set; }
    }
}