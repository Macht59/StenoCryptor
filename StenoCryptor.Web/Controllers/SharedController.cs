using System.IO;
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
            return File(fileName, contentType, Path.GetFileName(fileName));
        }

        #endregion Actions
    }
}
