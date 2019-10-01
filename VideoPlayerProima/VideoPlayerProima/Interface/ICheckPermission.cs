using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SetBoxTV.VideoPlayer.Interface
{
    public interface ICheckPermission
    {
        Task CheckSelfPermission();
    }
}
