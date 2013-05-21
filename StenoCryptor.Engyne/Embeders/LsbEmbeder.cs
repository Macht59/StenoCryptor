using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Commons.Enums;
using StenoCryptor.Engyne.Helpers;
using StenoCryptor.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StenoCryptor.Engyne.Embeders
{
    public class LsbEmbeder : IEmbeder
    {
        #region Public Logics

        public void Embed(Container container, byte[] message)
        {
            if (!container.ContentType.Contains(Constants.IMAGE_CONTENT_TYPE))
                throw new ArgumentException("LSB is only works with image containers.");

            Bitmap bitmap = new Bitmap(container.InputStream);

            if (bitmap.Width * bitmap.Height < message.Length << 1)
                throw new ArgumentException("Message is to big for this container.");

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int number = (x + 1) * (y + 1);
                    if (number > message.Length)
                    {
                        byte[] bytes = (byte[])(new ImageConverter().ConvertTo(bitmap, typeof(byte[])));
                        container.InputStream = new MemoryStream(bytes);
                        return;
                    }

                    Color pixel = bitmap.GetPixel(x, y);
                    pixel = processPixel(number, pixel, message);
                    bitmap.SetPixel(x, y, pixel);
                }
            }

            bitmap.Save(container.InputStream, ImageFormat.Bmp);
        }

        public byte[] Extract(Container container, Key key)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Logics

        private Color processPixel(int number, Color pixel, byte[] messageArray)
        {
            int charNumber = number / 2;
            byte byteToHide = messageArray[charNumber];
            int intPixel = pixel.ToArgb();

            if (charNumber % 2 == 1)
            {
                intPixel = setLastBit(intPixel, 1, (byteToHide >> 4) & 1);
                intPixel = setLastBit(intPixel, 2, (byteToHide >> 5) & 1);
                intPixel = setLastBit(intPixel, 3, (byteToHide >> 6) & 1);
                intPixel = setLastBit(intPixel, 4, (byteToHide >> 7) & 1);
            }
            else
            {
                intPixel = setLastBit(intPixel, 1, (byteToHide >> 1) & 1);
                intPixel = setLastBit(intPixel, 2, (byteToHide >> 2) & 1);
                intPixel = setLastBit(intPixel, 3, (byteToHide >> 3) & 1);
                intPixel = setLastBit(intPixel, 4, (byteToHide >> 4) & 1);
            }

            return Color.FromArgb(intPixel);
        }

        private int setLastBit(int target, byte byteNumber, int value)
        {
            switch (value)
            {
                case 0:
                    return unsetLastBit(target, byteNumber);
                case 1:
                    return setLastBit(target, byteNumber);
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        private static int setLastBit(int target, byte byteNumber)
        {
            switch (byteNumber)
            {
                case 1:
                    return target | 0x01000000;
                case 2:
                    return target | 0x00010000;
                case 3:
                    return target | 0x00000100;
                case 4:
                    return target | 0x00000001;
                default:
                    throw new ArgumentOutOfRangeException("byteNumber");
            }
        }

        private static int unsetLastBit(int target, byte byteNumber)
        {
            switch (byteNumber)
            {
                case 1:
                    return (int)((uint)target & 0xFEFFFFFF);
                case 2:
                    return (int)((uint)target & 0xFFFEFFFF);
                case 3:
                    return (int)((uint)target & 0xFFFFFEFF);
                case 4:
                    return (int)((uint)target & 0xFFFFFFFE);
                default:
                    throw new ArgumentOutOfRangeException("byteNumber");
            }
        }

        #endregion
    }
}
