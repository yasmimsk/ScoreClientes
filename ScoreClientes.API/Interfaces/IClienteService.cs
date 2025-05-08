using ScoreClientes.API.Models;

namespace ScoreClientes.API.Interfaces
{
    public interface IClienteService
    {
        string DadosValidos(string? estado, string? cep, string? ddd, string? telefone, string? cpf, string? email);
        int CalcularScore(Cliente cliente);
    }
}
