using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne
{
    public static class FormatFactory
    {
        public static ImageFormat GetInstance(string contentType)
        {
            switch (contentType)
            {
                case "image/jpg":
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "imge/png":
                    return ImageFormat.Png;
                default:
                    throw new NotSupportedException("Current content type is not supported.");
            }
        }
    }
}
