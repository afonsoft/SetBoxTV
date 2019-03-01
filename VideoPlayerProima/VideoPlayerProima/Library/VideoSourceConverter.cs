using System;
using Xamarin.Forms;

namespace VideoPlayerProima.Library
{
    public class VideoSourceConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                return Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme != "file" ?
                    VideoSource.FromUri(value) : VideoSource.FromFile(value);
            }

            throw new InvalidOperationException("Cannot convert null or whitespace to ImageSource");
        }
    }
}
