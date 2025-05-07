using ScoreClientes.API.Interfaces;
using ScoreClientes.API.Models;

namespace ScoreClientes.API.Services
{
    public class ClienteService : IClienteService
    {
        public int CalcularScore(Cliente cliente)
        {
            int score = 0;
            
            if (cliente.RendimentoAnual < 60000)
                score += 100;
            else if (cliente.RendimentoAnual <= 120000)
                score += 200;
            else
                score += 300;

            //Cálculo da idade - diminui um ano se ainda não fez aniversário
            var idade = DateTime.Today.Year - cliente.DataNascimento.Year;
            if (cliente.DataNascimento.DayOfYear > DateTime.Today.DayOfYear)
                idade--;

            if (idade < 25)
                score += 50;
            else if (idade <= 40)
                score += 150;
            else
                score += 200;

            return score;
        }
    }
}
