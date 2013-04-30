﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StenoCryptor.Web.Controllers
{
    public class SharedController : Controller
    {
        #region Constants

        public const string CONTROLLER = "Shared";

        public const string DOWNLOAD = "Download";

        public const string ERROR = "Error";

        #endregion Constants

        #region Actions

        [HttpGet]
        public FileResult Download(string fileName, string contentType)
        {
            return File(fileName, contentType);
        }

        #endregion Actions
    }
}