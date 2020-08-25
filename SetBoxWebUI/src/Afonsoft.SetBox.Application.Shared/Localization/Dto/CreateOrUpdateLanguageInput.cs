using System.ComponentModel.DataAnnotations;

namespace Afonsoft.SetBox.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}