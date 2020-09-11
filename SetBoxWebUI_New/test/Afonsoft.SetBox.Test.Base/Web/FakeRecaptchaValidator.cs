using System.Threading.Tasks;
using Afonsoft.SetBox.Security.Recaptcha;

namespace Afonsoft.SetBox.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
