﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models.Views

{
    public class FilesViewModel : BaseViewModel
    {
        public FilesViewModel(string messageError) : base(messageError)
        {

        }
        public FilesViewModel(Exception exception) : base(exception)
        {

        }

        public FilesViewModel() : base("")
        {

        }

        public FileCheckSum File { get; set; }

        public bool IsEdited { get; set; }

        public IList<Guid> Devices { get; set; }
        public IList<Guid> AllDevices { get; set; }
    }
}
