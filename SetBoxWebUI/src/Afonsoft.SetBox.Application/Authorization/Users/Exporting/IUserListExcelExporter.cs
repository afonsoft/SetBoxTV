using System.Collections.Generic;
using Afonsoft.SetBox.Authorization.Users.Dto;
using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}