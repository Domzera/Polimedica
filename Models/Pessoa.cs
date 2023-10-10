using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Polimedica.Models
{
    public class Pessoa:IdentityUser
    {
        //public int Id { get; set; }
        [Required,MaxLength(30)]
        public string PrimeiroNome { get; set; }
        [Required,MaxLength(30)]
        public string Password { get; set; }
        [Required,MaxLength(20)]
        public string funcao { get; set; }
    }
}
