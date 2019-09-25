using System;
using SetBoxTV.VideoPlayer.Library;

namespace SetBoxTV.VideoPlayer.Interface
{
    public interface IVideoPlayerController
    {
        VideoStatus Status { set; get; }

        TimeSpan Duration { set; get; }
    }
}
