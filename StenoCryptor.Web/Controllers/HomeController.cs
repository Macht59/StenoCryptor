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
                // process image

                return RedirectToAction(HomeControllerActions.RESULT);
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
        public ActionResult Download(string fileName)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Result()
        {
            if (!TempData.Keys.Contains(TempDataKeys.FILE_NAME))
            {
                TempData[TempDataKeys.ERROR] = StenoCryptor.Web.Localization.Views.Shared.FileAccessError;
                return View(SharedControllerActions.ERROR);
            }

            return RedirectToAction(HomeControllerActions.DOWNLOAD, HomeControllerActions.CONTROLLER, new { fileName = TempData[TempDataKeys.FILE_NAME] });
        }
    }
}
