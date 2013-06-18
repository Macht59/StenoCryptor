using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Commons.Enums;
using StenoCryptor.Engyne.Helpers;
using StenoCryptor.Enums.Commons;
using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StenoCryptor.Engyne.Embeders
{
    public class LsbEmbeder : AbstractEmbeder, IEmbeder
    {
        #region Constructors

        public LsbEmbeder(EmbedingOptions options)
        {
            _options = options;
        }

        #endregion Constructors

        #region Public Logics

        public void Embed(Container container, byte[] message)
        {
            if (!container.ContentType.Contains(Constants.IMAGE_CONTENT_TYPE))
                throw new ArgumentException("LSB is only works with image containers.");

            Bitmap bitmap = new Bitmap(container.InputStream);

            if (bitmap.Width * bitmap.Height < message.Length << 3)
                throw new ArgumentException("Message is to big for this container.");

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int number = x + y * bitmap.Width;

                    if (number >= message.Length << 3)
                    {
                        container.InputStream = SaveBitmap(bitmap, FormatFactory.GetInstance(container.ContentType));
                        return;
                    }
                    bitmap.SetPixel(x, y, insertDataIntoPixel(number, bitmap.GetPixel(x, y), message));
                }
            }

            container.InputStream = SaveBitmap(bitmap, FormatFactory.GetInstance(container.ContentType));
        }

        public byte[] Extract(Container container, Key key)
        {
            if (!container.ContentType.Contains(Constants.IMAGE_CONTENT_TYPE))
                throw new ArgumentException("LSB is only works with image containers.");

            Bitmap bitmap = new Bitmap(container.InputStream);
            byte[] bytes = new byte[key.MessageLength];

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int number = x + y * bitmap.Width;

                    if (number >= key.MessageLength << 3)
                    {
                        return bytes;
                    }

                    Color pixel = bitmap.GetPixel(x, y);
                    extractDataFromPixel(number, pixel, bytes);
                }
            }

            return bytes;
        }

        #endregion

        #region Private Logics

        private Color insertDataIntoPixel(int pixelNumber, Color pixel, byte[] messageArray)
        {
            byte blue = pixel.B;
            int hideCharNumber = pixelNumber >> 3;
            int bitNumber = pixelNumber % 8;

            if (((messageArray[hideCharNumber] >> (7 - bitNumber)) & 1) == 1)
            {
                blue |= 1;
            }
            else
            {
                blue &= 0xFE;
            }

            return Color.FromArgb(pixel.R, pixel.G, blue);
        }

        private void extractDataFromPixel(int pixelNumber, Color pixel, byte[] messageArray)
        {
            byte blue = pixel.B;
            int hideCharNamber = pixelNumber >> 3;
            int bitNumber = pixelNumber % 8;

            if ((blue & 1) == 1)
            {
                messageArray[hideCharNamber] |= (byte)(1 << (7 - bitNumber));
            }
            else
            {
                messageArray[hideCharNamber] &= (byte)(0xFF ^ (1 << (7 - bitNumber)));
            }
        }

        #endregion

        #region Fields

        private EmbedingOptions _options;

        #endregion
    }
}
