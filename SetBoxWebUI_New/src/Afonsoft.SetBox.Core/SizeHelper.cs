using System;

namespace Afonsoft.SetBox
{
    public static class SizeHelper
    {
        public static string FormatsonSize(double fileSizeInBytes)
        {
            int i = -1;
            string[] byteUnits = { " KB", " MB", " GB", " TB", "PB", "EB", "ZB", "YB" };
            do
            {
                fileSizeInBytes = fileSizeInBytes / 1024;
                i++;
            } while (fileSizeInBytes > 1024);

            return Math.Max(fileSizeInBytes, 0.1).ToString("N2") + byteUnits[i];
        }
    }
}
