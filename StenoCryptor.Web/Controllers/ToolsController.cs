using StenoCryptor.Commons.Constants;
using StenoCryptor.Web.Helpers;
using StenoCryptor.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StenoCryptor.Web.Controllers
{
    public class ToolsController : Controller
    {
        #region Constants

        public const string CONTROLLER = "Tools";

        public const string INDEX = "Index";

        public const string COMPARE_STRINGS = "CompareStrings";

        public const string COMPARE_STRING_RESULT = "CompareStringResult";

        #endregion Constants

        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CompareStrings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CompareStrings(CompareStringsModel model)
        {
            TempData[TempDataKeys.COMPARE_STRING_RESULT] = StringComparerHelper.Compare(model.String1, model.String2);
            return RedirectToAction(COMPARE_STRING_RESULT);
        }

        public ActionResult CompareStringResult()
        {
            return View();
        }

        #endregion Actions

    }
}
