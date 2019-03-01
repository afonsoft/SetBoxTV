using System;
using VideoPlayerProima.Library;

namespace VideoPlayerProima.Interface
{
    public interface IVideoPlayerController
    {
        VideoStatus Status { set; get; }

        TimeSpan Duration { set; get; }
    }
}
