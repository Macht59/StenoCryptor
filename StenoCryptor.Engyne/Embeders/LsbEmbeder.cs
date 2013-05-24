using StenoCryptor.Commons;
using StenoCryptor.Commons.Constants;
using StenoCryptor.Commons.Enums;
using StenoCryptor.Engyne.Helpers;
using StenoCryptor.Interfaces;
using System;
using System.Collections.Generic;
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
            bitmap.MakeTransparent();

            if (bitmap.Width * bitmap.Height < message.Length << 1)
                throw new ArgumentException("Message is to big for this container.");

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int number = (x + 1) * (y + 1);
                    if (number > message.Length << 1)
                    {
                        byte[] bytes = (byte[])(new ImageConverter().ConvertTo(bitmap, typeof(byte[])));
                        container.InputStream = new MemoryStream(bytes);
                        return;
                    }

                    Color pixel = bitmap.GetPixel(x, y);
                    pixel = insertDataInPixel(number, pixel, message);
                    bitmap.SetPixel(x, y, pixel);
                }
            }

            bitmap.Save(container.InputStream, ImageFormat.Bmp);
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
                    int number = (x + 1) * (y + 1);
                    if (number > key.MessageLength << 1)
                    {
                        return bytes;
                    }

                    Color pixel = bitmap.GetPixel(x, y);
                    extractDataFromPixel(pixel, number, bytes);
                }
            }

            return bytes;
        }

        #endregion

        #region Private Logics

        private Color insertDataInPixel(int number, Color pixel, byte[] messageArray)
        {
            int charNumber = (number - 1) >> 1;
            byte byteToHide = messageArray[charNumber];
            int intPixel = pixel.ToArgb();

            if ((number & 1) == 1)
            {
                intPixel = setLastBit(intPixel, 1, (byteToHide >> 7) & 1);
                intPixel = setLastBit(intPixel, 2, (byteToHide >> 6) & 1);
                intPixel = setLastBit(intPixel, 3, (byteToHide >> 5) & 1);
                intPixel = setLastBit(intPixel, 4, (byteToHide >> 4) & 1);
            }
            else
            {
                intPixel = setLastBit(intPixel, 1, (byteToHide >> 3) & 1);
                intPixel = setLastBit(intPixel, 2, (byteToHide >> 2) & 1);
                intPixel = setLastBit(intPixel, 3, (byteToHide >> 1) & 1);
                intPixel = setLastBit(intPixel, 4, byteToHide & 1);
            }

            return Color.FromArgb(intPixel);
        }

        private void extractDataFromPixel(Color pixel, int number, byte[] message)
        {
            int intPixel = pixel.ToArgb();
            int charNumber = (number - 1) >> 1;

            if ((number & 1) == 1)
            {
                message[charNumber] |= (byte)(getLastBit(intPixel, 1) << 7);
                message[charNumber] |= (byte)(getLastBit(intPixel, 2) << 6);
                message[charNumber] |= (byte)(getLastBit(intPixel, 3) << 5);
                message[charNumber] |= (byte)(getLastBit(intPixel, 4) << 4);
            }
            else
            {
                message[charNumber] |= (byte)(getLastBit(intPixel, 1) << 3);
                message[charNumber] |= (byte)(getLastBit(intPixel, 2) << 2);
                message[charNumber] |= (byte)(getLastBit(intPixel, 3) << 1);
                message[charNumber] |= (byte)getLastBit(intPixel, 4);
            }
        }

        private int getLastBit(int value, int number)
        {
            switch (number)
            {
                case 1:
                    return (value >> 24) & 1;
                case 2:
                    return (value >> 16) & 1;
                case 3:
                    return (value >> 8) & 1;
                case 4:
                    return value & 1;
                default: 
                    throw new ArgumentOutOfRangeException("number");
            }
        }

        private int setLastBit(int target, int byteNumber, int value)
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

        private static int setLastBit(int target, int byteNumber)
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

        private static int unsetLastBit(int target, int byteNumber)
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
