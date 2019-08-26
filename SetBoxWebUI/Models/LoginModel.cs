using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public class LoginModel
    {
        public int Id { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "Informe o nome do usuário", AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Informe a senha do usuário", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Lembra-me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
