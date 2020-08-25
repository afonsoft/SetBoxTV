using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace Afonsoft.SetBox.Net.Emailing
{
    public class SetBoxSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public SetBoxSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}