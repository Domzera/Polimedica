using Polimedica.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace Polimedica.Models
{
    public class Roteiro
    {
        public int Id { get; set; }
        [Timestamp]
        public string? Checado { get; set; }
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        [Required,MaxLength(30)]
        public string? Cliente { get; set; }
        [Required, MaxLength(20)]
        public string? Cidade { get; set; }
        [Required,MaxLength(50)]
        public string? Observacao { get; set; }
        [MaxLength(15)]
        public string? Cartao { get; set; }
        [Required]
        public LojaPoli LojaPolimedica { get; set; }
        [MaxLength(20)]
        public string? PedidoNF { get; set; }
        [MaxLength(10)]
        public string? DinheiroCheque { get; set; }
        [MaxLength(10)]
        public string? Troco { get; set; }
        [Required]
        public Periodo? Periodo { get; set;}
    }
}
