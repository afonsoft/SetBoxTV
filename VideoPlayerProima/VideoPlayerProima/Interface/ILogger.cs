using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPlayerProima.Interface
{
    public interface ILogger
    {
        void Info(string text);
        void Debug(string text);
        void Error(string text, Exception ex);
        void Error(Exception ex);
    }
}
