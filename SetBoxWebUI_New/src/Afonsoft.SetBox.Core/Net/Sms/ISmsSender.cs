using System.Threading.Tasks;

namespace Afonsoft.SetBox.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}