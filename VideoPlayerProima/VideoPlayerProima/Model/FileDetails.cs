using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPlayerProima.Model
{
    public class FileDetails
    {
        public string Path { get; set; }
        public EnumFileType FileType { get; set; }
        public string CheckSum { get; set; }
    }

    public enum EnumFileType
    {
        Video,
        Image,
        WebImage,
        WebPage,
        WebVideo
    }
}
