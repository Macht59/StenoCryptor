using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StenoCryptor.Engyne.Embeders
{
    public class AbstractEmbeder
    {
        protected Stream SaveBitmap(Bitmap bitmap, ImageFormat imageFormat)
        {
            byte[] bytes = (byte[])(new ImageConverter().ConvertTo(bitmap, typeof(byte[])));
            Bitmap bmp = new Bitmap(new MemoryStream(bytes));
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, imageFormat);
            return ms;
        }
    }
}
