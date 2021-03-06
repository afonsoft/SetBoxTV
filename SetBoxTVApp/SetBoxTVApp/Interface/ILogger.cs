﻿using System;
using System.Collections.Generic;

namespace SetBoxTVApp.Interface
{
    public interface ILogger
    {
        string TAG { get; set; }
        string DeviceIdentifier { get; set; }
        string Platform { get; set; }
        string Version { get; set; }

        bool IsDebugEnabled { get; set; }

        string LogFileName { get; }
        string LogFileContent { get; }

        void Debug(string text);
        void Debug(string text, Dictionary<string, string> prop);
        void Debug(string text, Exception ex);
        void Error(string text, Exception ex);
        void Error(Exception ex);
        void Error(string text);
        void ErrorVLC(string text);
    }
}
