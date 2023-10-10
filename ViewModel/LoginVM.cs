using System.ComponentModel.DataAnnotations;

namespace Polimedica.ViewModel
{
    public class LoginVM
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "É nescessário um nome!")]
        public string Nome { get; set; }
        [Display(Name = "Senha")]
        [Required(ErrorMessage ="Senha invalida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
