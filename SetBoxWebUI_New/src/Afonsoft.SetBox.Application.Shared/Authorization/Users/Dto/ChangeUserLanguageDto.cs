using System.ComponentModel.DataAnnotations;

namespace Afonsoft.SetBox.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
