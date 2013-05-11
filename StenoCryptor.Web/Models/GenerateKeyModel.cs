using StenoCryptor.Commons.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Models
{
    public class GenerateKeyModel
    {
        public CryptType CryptType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Models.GenerateKeyModel), ErrorMessageResourceName = "errKeyRequired")]
        public string Key { get; set; }
    }
}