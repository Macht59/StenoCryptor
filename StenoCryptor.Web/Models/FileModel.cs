
namespace StenoCryptor.Web.Models
{
    public class FileModel
    {
        public FileModel()
        {

        }

        public FileModel(string fileName, string contentType)
        {
            this.FileName = fileName;
            this.ContentType = contentType;
        }

        public string FileName { get; set; }

        public string ContentType { get; set; }
    }
}