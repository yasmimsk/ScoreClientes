using System.ComponentModel.DataAnnotations;

namespace ScoreClientes.API.Models
{
    public class Cliente
    {

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string Cpf { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public decimal RendimentoAnual { get; set; }

        [Required]
        public string Endereco { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public string Cep { get; set; }

        [Required]
        public string Ddd { get; set; }

        [Required]
        public string Telefone { get; set; }
    }
}
