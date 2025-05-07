using ScoreClientes.API.Models;

namespace ScoreClientes.API.Interfaces
{
    public interface IClienteService
    {
        int CalcularScore(Cliente cliente);
    }
}
