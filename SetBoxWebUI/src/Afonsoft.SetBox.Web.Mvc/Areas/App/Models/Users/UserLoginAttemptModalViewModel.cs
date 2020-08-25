using System.Collections.Generic;
using Afonsoft.SetBox.Authorization.Users.Dto;

namespace Afonsoft.SetBox.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}