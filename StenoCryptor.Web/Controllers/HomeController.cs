﻿using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Interfaces;
using StenoCryptor.Web.Helpers;
using StenoCryptor.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        public const string EMBED_RESULT = "EmbedResult";

        public const string DETECT_RESULT = "DetectResult";

        public const string EXTRACT_RESULT = "ExtractResult";

        public const string GENERATE = "Generate";

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
        public ActionResult Embed(Models.DwmModel model, HttpPostedFileBase photoFile)
        {
            if (photoFile == null)
                ModelState.AddModelError("photoFile", Localization.Views.Shared.FileIsNotSelected);

            if (ModelState.IsValid)
            {
                try
                {
                    ICryptor cryptor = _algorithmFactory.GetInstance(model.CryptType);
                    IEmbeder embeder = _embederFactory.GetInstance(model.EmbedType);
                    Container container = new Container(photoFile.InputStream, photoFile.ContentType);
                    Key key = KeyMakerHelper.GenerateKey(container, model.Message, model.CryptType, cryptor, model.Key);
                    DwmProcessorHelper.EmbedDwm(cryptor, embeder, model.Message, key, container);
                    Stream keyStream = SerializeHelper.SerializeBinary(key);
                    Dictionary<string, Stream> files = new Dictionary<string, Stream>();
                    files.Add(Path.GetFileName(photoFile.FileName), container.InputStream);
                    files.Add(Constants.DEFAULT_KEY_NAME, keyStream);
                    string zipFileName = ZipHelper.CompressFiles(files);
                    
                    TempData[TempDataKeys.FILE_NAME] = new FileModel(zipFileName, photoFile.ContentType);

                    return RedirectToAction(HomeController.EMBED_RESULT);
                }
                catch (Exception ex)
                {
                    TempData[TempDataKeys.ERROR] = ex.Message;
                    return View(SharedController.ERROR);
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult EmbedResult()
        {
            if (!TempData.Keys.Contains(TempDataKeys.FILE_NAME))
            {
                TempData[TempDataKeys.ERROR] = StenoCryptor.Web.Localization.Views.Shared.FileAccessError;
                return View(SharedController.ERROR);
            }

            return View((FileModel)TempData[TempDataKeys.FILE_NAME]);
        }

        [HttpGet]
        public ActionResult Detect()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Detect(HttpPostedFileBase photoFile)
        {
            if (photoFile == null || photoFile.InputStream.Length == 0)
                ModelState.AddModelError("photoFile", Localization.Views.Home.ErrPostFileIsNullOrEmpty);

            if (ModelState.IsValid)
            {
                IDetector detector = _detectorFactory.GetInstance();
                TempData[TempDataKeys.DETECT_RESULT] = DwmProcessorHelper.DetectDwm(detector, photoFile.InputStream);

                return RedirectToAction(DETECT_RESULT);
            }

            return View();
        }

        [HttpGet]
        public ActionResult DetectResult()
        {
            if (TempData[TempDataKeys.DETECT_RESULT] == null)
            {
                TempData[TempDataKeys.ERROR] = Localization.Views.Home.AccessError;
                return View(SharedController.ERROR);
            }

            ViewBag.Detected = TempData[TempDataKeys.DETECT_RESULT];

            return View();
        }

        [HttpGet]
        public ActionResult Extract()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Extract(HttpPostedFileBase photoFile)
        {
            if (photoFile == null || photoFile.InputStream.Length == 0)
                ModelState.AddModelError("photoFile", Localization.Views.Home.ErrPostFileIsNullOrEmpty);

            if (ModelState.IsValid)
            {
                Container container = new Container(photoFile.InputStream, photoFile.ContentType);
                DwmModel model = DwmProcessorHelper.ExtractDwm(container, null);

                TempData[TempDataKeys.FILE_NAME] = StreamHelper.SaveFile(container.InputStream, Path.GetFileName(photoFile.FileName));
                //TempData[TempDataKeys.CONTENT_TYPE] = photoFile.ContentType;
                TempData[TempDataKeys.EXTRACTED_MODEL] = model;

                return RedirectToAction(EXTRACT_RESULT);
            }

            return View();
        }

        [HttpGet]
        public ActionResult ExtractResult()
        {
            if (TempData[TempDataKeys.FILE_NAME] == null ||
                TempData[TempDataKeys.EXTRACTED_MODEL] == null)
            {
                TempData[TempDataKeys.ERROR] = Localization.Views.Home.AccessError;
                return View(SharedController.ERROR);
            }

            ViewBag.FileName = TempData[TempDataKeys.FILE_NAME];
            //ViewBag.ContentType = TempData[TempDataKeys.CONTENT_TYPE];

            return View(TempData[TempDataKeys.EXTRACTED_MODEL]);
        }

        #endregion Actions

        #region Fields

        private IAlgorithmFactory _algorithmFactory;
        private IEmbederFactory _embederFactory;
        private IDetectorFactory _detectorFactory;

        #endregion Fields
    }
}
