using Microsoft.VisualBasic;
using Polimedica.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace Polimedica.ViewModel
{
    public class CriarVM
    {
        public int Id { get; set; }
        //[DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public string Cliente { get; set; }
        public string Cidade { get; set; }
        public string Observacao { get; set; }
        public string? Cartao { get; set; }
        public LojaPoli LojaPolimedica { get; set; }
        public string? PedidoNF { get; set; }
        public string? DinheiroCheque { get; set; }
        public string? Troco { get; set; }
        public Periodo Periodo { get; set; }
    }
}
