using ScoreClientes.API.Models;

namespace ScoreClientes.API.Interfaces
{
    public interface IClienteRepository
    {
        public void InserirCliente(Cliente cliente);
        IList<Cliente> ObterTodos();
        Cliente ObterPorId(int id);
        Cliente ObterPorCpf(string cpf);
        Cliente ObterPorEmail(string email);
        IList<Cliente> ObterPorEstado(string estado);
        void Atualizar(int id, Cliente cliente);
        void Deletar(int id);
    }
}
