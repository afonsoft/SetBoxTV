using System.Collections.Generic;
using Afonsoft.SetBox.Chat.Dto;
using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(List<ChatMessageExportDto> messages);
    }
}
