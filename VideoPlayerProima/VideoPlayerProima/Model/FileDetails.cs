using System;
using System.Collections.Generic;
using System.Text;

namespace SetBoxTV.VideoPlayer.Model
{
    public class FileDetails : FileCheckSum
    {
        public string path { get; set; }
     
        public EnumFileType fileType { get; set; }
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
