using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace StenoCryptor.Web.Helpers
{
    public static class ZipHelper
    {
        public static string CompressFiles(IDictionary<string, Stream> files)
        {
            string guid = Guid.NewGuid().ToString();
            string tempDirecotryName = Path.GetTempPath();
            string zipArchiveName = Path.Combine(tempDirecotryName, guid + ".zip");
            string directoryName = Path.Combine(tempDirecotryName, guid);

            if (Directory.Exists(directoryName))
                Directory.Delete(directoryName, true);

            Directory.CreateDirectory(directoryName);

            foreach (KeyValuePair<string, Stream> file in files)
            {
                StreamHelper.SaveFile(file.Value, Path.Combine(directoryName, Path.GetFileName(file.Key)));
            }

            ZipFile.CreateFromDirectory(directoryName, zipArchiveName);

            Directory.Delete(directoryName, true);

            return zipArchiveName;
        }
    }
}