using System;
using System.Text;
using System.Threading.Tasks;

namespace SetBoxTV.VideoPlayer.Interface
{
    public interface ICheckPermission
    {
        Task CheckSelfPermission();
    }
}
