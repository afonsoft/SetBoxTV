using System.Threading.Tasks;
using Afonsoft.SetBox.Sessions.Dto;

namespace Afonsoft.SetBox.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
