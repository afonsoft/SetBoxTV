using Abp.Domain.Services;

namespace Afonsoft.SetBox
{
    public abstract class SetBoxDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected SetBoxDomainServiceBase()
        {
            LocalizationSourceName = SetBoxConsts.LocalizationSourceName;
        }
    }
}
