using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository
{
    public class FileDeviceRepository
    {
        private readonly ApplicationDbContext context;
        public FileDeviceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task UpdateOrder(string deviceId, string fileId, int order)
        {
            var upd = this.context.FilesDevices.FirstOrDefault(x => x.DeviceId.ToString() == deviceId && x.FileId.ToString() == fileId);
            if (upd != null)
            {
                upd.Order = order;
                await this.context.SaveChangesAsync();
            }
        }
    }
}