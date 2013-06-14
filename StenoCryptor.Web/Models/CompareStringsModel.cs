using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Models
{
    public class CompareStringsModel
    {
        [Required]
        public string String1 { get; set; }

        [Required]
        public string String2 { get; set; }
    }
}