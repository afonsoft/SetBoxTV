using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
