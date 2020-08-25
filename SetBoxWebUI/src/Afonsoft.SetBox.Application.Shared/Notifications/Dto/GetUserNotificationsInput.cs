using System;
using Abp.Notifications;
using Afonsoft.SetBox.Dto;

namespace Afonsoft.SetBox.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}