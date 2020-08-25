using System.Collections.Generic;
using Afonsoft.SetBox.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Afonsoft.SetBox.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
