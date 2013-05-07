using Microsoft.Practices.Unity;
using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Commons.Helpers;
using StenoCryptor.Interfaces;
using System;
using System.IO;
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

        #region Constructors

        public HomeController(IAlgorithmFactory algorithmFactory, IEmbederFactory embederFactory, IDetectorFactory detectorFactory)
        {
            _algorithmFactory = algorithmFactory;
            _embederFactory = embederFactory;
            _detectorFactory = detectorFactory;
        }

        #endregion Constructors

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
                try
                {
                    processFile(model, photoFile);
                }
                catch (Exception ex)
                {
                    TempData[TempDataKeys.ERROR] = ex.Message;
                    return View(SharedController.ERROR);
                }

                TempData[TempDataKeys.FILE_NAME] = FileHelper.SaveFile(photoFile.InputStream, Path.GetFileName(photoFile.FileName));
                TempData[TempDataKeys.CONTENT_TYPE] = photoFile.ContentType;

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

            return View(new Models.File()
            {
                FileName = (string)TempData[TempDataKeys.FILE_NAME],
                ContentType = (string)TempData[TempDataKeys.CONTENT_TYPE]
            });
        }

        #endregion Actions

        #region Private Logics

        private void processFile(Models.DwmEmbedModel model, HttpPostedFileBase photoFile)
        {
            ICryptor cryptor = _algorithmFactory.GetInstance(model.CryptType);
            Container container = new Container() { Data = photoFile.InputStream };

            cryptor.Encrypt(container,
                new Key() { Value = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 } });


        }

        #endregion Private Logics

        #region Fields

        private IAlgorithmFactory _algorithmFactory;
        private IEmbederFactory _embederFactory;
        private IDetectorFactory _detectorFactory;

        #endregion Fields
    }
}
