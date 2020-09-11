using System.Collections.Generic;
using Afonsoft.SetBox.Authorization.Users.Importing.Dto;
using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
