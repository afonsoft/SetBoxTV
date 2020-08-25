using System.Collections.Generic;
using Afonsoft.SetBox.Auditing.Dto;
using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
