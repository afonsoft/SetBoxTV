using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class OrderDto 
    {
        public OrderDto()
        {
            fileOrders = new List<FileOrder>();
        }
        [Required]
        public long DeviceId { get; set; }  
        public List<FileOrder> fileOrders { get; set; }
    }
    public class FileOrder
    {
        [Required]
        public long FileId { get; set; }
        [Required]
        public int Order { get; set; }
    }
}