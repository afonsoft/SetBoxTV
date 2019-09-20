﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPlayerProima.Model
{
    public class ConfigModel
    {
        /*
            configId	string($uuid)
            enableVideo	boolean
            enablePhoto	boolean
            enableWebVideo	boolean
            enableWebImage	boolean
            enableTransaction	boolean
            transactionTime	integer($int32)
            creationDateTime	string($date-time)
        */

        public Guid configId { get; set; }
        public bool enableVideo { get; set; }
        public bool enablePhoto { get; set; }
        public bool enableWebVideo { get; set; }
        public bool enableWebImage { get; set; }
        public bool enableTransaction { get; set; }
        public int transactionTime { get; set; }
        public DateTime creationDateTime { get; set; }
    }
}