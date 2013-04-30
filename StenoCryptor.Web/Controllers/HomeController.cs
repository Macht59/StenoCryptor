using StenoCryptor.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StenoCryptor.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Constants

        public const string CONTROLLER = "Home";

        public const string INDEX = "Index";

        public const string EMBED = "Embed";

        public const string EXTRACT = "Extract";

        public const string DETECT = "Detect";

        public const string RESULT = "RESULT";

        #endregion Constants

        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Embed()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Embed(Models.DwmEmbedModel model, HttpPostedFileBase photoFile)
        {
            if (photoFile == null)
                ModelState.AddModelError("photoFile", Localization.Views.Shared.FileIsNotSelected);

            if (ModelState.IsValid)
            {
                TempData[TempDataKeys.FILE_NAME] = FileHelper.SaveFile(photoFile.InputStream, Path.GetFileName(photoFile.FileName));
                TempData[TempDataKeys.CONTENT_TYPE] = photoFile.ContentType;
                // process image

                return RedirectToAction(HomeController.RESULT);
            }

            return View();
        }

        [HttpGet]
        public ActionResult Extract()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detect()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Result()
        {
            if (!TempData.Keys.Contains(TempDataKeys.FILE_NAME))
            {
                TempData[TempDataKeys.ERROR] = StenoCryptor.Web.Localization.Views.Shared.FileAccessError;
                return View(SharedController.ERROR);
            }

            return RedirectToAction(SharedController.DOWNLOAD, SharedController.CONTROLLER, new { fileName = TempData[TempDataKeys.FILE_NAME], contentType = TempData[TempDataKeys.CONTENT_TYPE] });
        }

        #endregion Actions
    }
}
