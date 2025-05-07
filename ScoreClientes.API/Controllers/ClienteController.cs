using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ScoreClientes.API.Interfaces;
using ScoreClientes.API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ScoreClientes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _repository;
        private readonly IClienteService _service;

        public ClienteController(IClienteRepository repository, IClienteService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo cliente.")]
        [SwaggerResponse(200, "Cliente cadastrado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos ou CPF/Email incorretos.")]
        [SwaggerResponse(500, "Erro interno ao inserir cliente.")]
        public IActionResult CadastrarCliente([FromBody] Cliente cliente)
        {
            #region Validações
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (cliente.Estado.Length != 2)
                return BadRequest("Estado inválido.");

            if (cliente.Cep.Length != 8)
                return BadRequest("CEP inválido.");

            if (cliente.Ddd.Length != 2)
                return BadRequest("DDD inválido.");

            if (cliente.Telefone.Length != 8 && cliente.Telefone.Length != 9)
                return BadRequest("Telefone inválido.");

            if (!CpfValido(cliente.Cpf))
                return BadRequest("CPF inválido.");

            if (!EmailValido(cliente.Email))
                return BadRequest("Email inválido.");

            if (_repository.ObterPorCpf(cliente.Cpf) != null)
                return BadRequest("CPF já cadastrado.");

            if (_repository.ObterPorEmail(cliente.Email) != null)
                return BadRequest("Email já cadastrado.");
            #endregion

            try
            {
                _repository.InserirCliente(cliente);
                return Ok("Cliente cadastrado com sucesso.");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao inserir cliente: " + ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Obtém todos os clientes cadastrados.")]
        [SwaggerResponse(200, "Lista de clientes retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno ao obter clientes.")]
        public IActionResult ObterTodos()
        {
            try
            {
                IList<Cliente> clientes = _repository.ObterTodos();
                return Ok(clientes);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao obter clientes: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém um cliente pelo ID.")]
        [SwaggerResponse(200, "Cliente encontrado.")]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        [SwaggerResponse(500, "Erro interno ao obter cliente.")]
        public IActionResult ObterPorId(int id)
        {
            try
            {
                Cliente cliente = _repository.ObterPorId(id);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");
                return Ok(cliente);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao obter cliente: " + ex.Message);
            }
        }

        [HttpGet("cpf/{cpf}")]
        [SwaggerOperation(Summary = "Obtém um cliente pelo CPF.")]
        [SwaggerResponse(200, "Cliente encontrado.")]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        [SwaggerResponse(500, "Erro interno ao obter cliente.")]
        public IActionResult ObterPorCpf(string cpf)
        {
            try
            {
                Cliente cliente = _repository.ObterPorCpf(cpf);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");
                return Ok(cliente);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao obter cliente: " + ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        [SwaggerOperation(Summary = "Obtém um cliente pelo email.")]
        [SwaggerResponse(200, "Cliente encontrado.")]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        [SwaggerResponse(500, "Erro interno ao obter cliente.")]
        public IActionResult ObterPorEmail(string email)
        {
            try
            {
                Cliente cliente = _repository.ObterPorEmail(email);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");
                return Ok(cliente);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao obter cliente: " + ex.Message);
            }
        }

        [HttpGet("estado/{estado}")]
        [SwaggerOperation(Summary = "Obtém todos os clientes de um estado.")]
        [SwaggerResponse(200, "Clientes encontrados.")]
        [SwaggerResponse(500, "Erro interno ao obter clientes.")]
        public IActionResult ObterPorEstado(string estado)
        {
            try
            {
                IList<Cliente> clientes = _repository.ObterPorEstado(estado);
                return Ok(clientes);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao obter clientes: " + ex.Message);
            }
        }

        [HttpPatch("{id}")]
        [SwaggerOperation(Summary = "Atualiza os dados de um cliente.")]
        [SwaggerResponse(200, "Cliente atualizado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos.")]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        [SwaggerResponse(500, "Erro interno ao atualizar cliente.")]
        public IActionResult AtualizarCliente(int id, [FromBody] ClienteAtualizacao clienteAtualizacao)
        {
            try
            {
                Cliente cliente = _repository.ObterPorId(id);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");

                #region Validações e preenchimento da classe
                if (!string.IsNullOrEmpty(clienteAtualizacao.Email))
                {
                    if (!EmailValido(clienteAtualizacao.Email))
                        return BadRequest("Email inválido.");
                    cliente.Email = clienteAtualizacao.Email;
                }
                if (clienteAtualizacao.RendimentoAnual.HasValue)
                    cliente.RendimentoAnual = clienteAtualizacao.RendimentoAnual.Value;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Endereco))
                    cliente.Endereco = clienteAtualizacao.Endereco;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Cidade))
                    cliente.Cidade = clienteAtualizacao.Cidade;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Estado))
                {
                    if (clienteAtualizacao.Estado.Length != 2)
                        return BadRequest("Estado inválido.");
                    cliente.Estado = clienteAtualizacao.Estado;
                }
                if (!string.IsNullOrEmpty(clienteAtualizacao.Cep))
                {
                    if (clienteAtualizacao.Cep.Length != 8)
                        return BadRequest("CEP inválido.");
                    cliente.Cep = clienteAtualizacao.Cep;
                }
                if (!string.IsNullOrEmpty(clienteAtualizacao.Ddd))
                {
                    if (clienteAtualizacao.Ddd.Length != 2)
                        return BadRequest("DDD inválido.");
                    cliente.Ddd = clienteAtualizacao.Ddd;
                }
                if (!string.IsNullOrEmpty(clienteAtualizacao.Telefone))
                {
                    if (clienteAtualizacao.Telefone.Length != 8 && clienteAtualizacao.Telefone.Length != 9)
                        return BadRequest("Telefone inválido.");
                    cliente.Telefone = clienteAtualizacao.Telefone;
                }
                #endregion

                _repository.Atualizar(id, cliente);
                return Ok("Cliente atualizado com sucesso.");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao atualizar cliente: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove um cliente pelo ID.")]
        [SwaggerResponse(200, "Cliente removido com sucesso.")]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        [SwaggerResponse(500, "Erro interno ao remover cliente.")]
        public IActionResult DeletarCliente(int id)
        {
            try
            {
                Cliente cliente = _repository.ObterPorId(id);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");

                _repository.Deletar(id);
                return Ok("Cliente removido com sucesso.");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao remover cliente: " + ex.Message);
            }
        }

        [HttpGet("score/{cpf}")]
        [SwaggerOperation(Summary = "Obtém o score de um cliente.")]
        [SwaggerResponse(200, "Score do cliente.")]
        [SwaggerResponse(404, "Cliente não encontrado.")]
        [SwaggerResponse(500, "Erro interno ao obter score.")]
        public IActionResult ObterScore(string cpf)
        {
            try
            {
                Cliente cliente = _repository.ObterPorCpf(cpf);
                if (cliente == null)
                    return NotFound("Cliente não encontrado.");

                int score = _service.CalcularScore(cliente);

                return Ok(score);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Erro ao obter score: " + ex.Message);
            }
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
    }
}
