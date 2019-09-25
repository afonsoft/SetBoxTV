using System;
using System.Collections.Generic;
using System.Text;

namespace SetBoxTV.VideoPlayer.Interface
{
    public interface IClipboardService
    {
        string GetTextFromClipboard();
        void SendTextToClipboard(string text);
    }
}
