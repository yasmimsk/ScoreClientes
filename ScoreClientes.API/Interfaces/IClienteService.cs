using ScoreClientes.API.Models;

namespace ScoreClientes.API.Interfaces
{
    public interface IClienteService
    {
        bool CpfValido(string cpf);
        bool EmailValido(string email);
        int CalcularScore(Cliente cliente);
    }
}
