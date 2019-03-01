using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPlayerProima.Interface
{
    public interface IClipboardService
    {
        string GetTextFromClipboard();
        void SendTextToClipboard(string text);
    }
}
