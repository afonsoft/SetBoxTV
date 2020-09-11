using System.Threading.Tasks;

namespace Afonsoft.SetBox.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}