using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPlayerProima.Interface
{
    public interface ILogger
    {
        void Info(string text);
        void Info(string text, Exception ex);
        void Info(Exception ex);
        void Error(string text);
        void Error(string text, Exception ex);
        void Error(Exception ex);
    }
}
