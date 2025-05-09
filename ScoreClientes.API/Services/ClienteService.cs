﻿using ScoreClientes.API.Interfaces;
using ScoreClientes.API.Models;

namespace ScoreClientes.API.Services
{
    public class ClienteService : IClienteService
    {
        public string DadosValidos(string? estado, string? cep, string? ddd, string? telefone, string? cpf, string? email)
        {
            if (!string.IsNullOrEmpty(cpf) && !CpfValido(cpf))
                return "CPF inválido.";

            if (!string.IsNullOrEmpty(email) && !EmailValido(email))
                return "Email inválido.";

            if (!string.IsNullOrEmpty(estado) && estado.Length != 2)
                return "Estado inválido.";

            if (!string.IsNullOrEmpty(cep) && cep.Length != 8)
                return "CEP inválido.";

            if (!string.IsNullOrEmpty(ddd) && ddd.Length != 2)
                return "DDD inválido.";

            if (!string.IsNullOrEmpty(telefone) && telefone.Length != 8 && telefone.Length != 9)
                return "Telefone inválido.";

            return string.Empty;
        }

        private bool CpfValido(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * mult1[i];

            int resto = sum % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            tempCpf += resto;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * mult2[i];

            resto = sum % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            tempCpf += resto;

            return cpf == tempCpf;
        }

        private bool EmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

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
