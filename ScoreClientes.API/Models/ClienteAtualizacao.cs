using System.ComponentModel.DataAnnotations;

namespace ScoreClientes.API.Models
{
    public class ClienteAtualizacao
    {
        public string? Email { get; set; }
        public decimal? RendimentoAnual { get; set; }
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Cep { get; set; }
        public string? Ddd { get; set; }
        public string? Telefone { get; set; }
    }
}
