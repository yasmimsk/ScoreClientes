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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string erroValidacao = _service.DadosValidos(cliente.Estado, cliente.Cep, cliente.Ddd, cliente.Telefone, cliente.Cpf, cliente.Email);
            if (!string.IsNullOrEmpty(erroValidacao))
                return BadRequest(erroValidacao);

            if (_repository.CpfOuEmailJaCadastrado(cliente.Cpf, cliente.Email))
                return BadRequest("CPF ou email já cadastrado.");

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

                string erroValidacao = _service.DadosValidos(clienteAtualizacao.Estado, clienteAtualizacao.Cep, clienteAtualizacao.Ddd, clienteAtualizacao.Telefone, null, clienteAtualizacao.Email);
                if (!string.IsNullOrEmpty(erroValidacao))
                    return BadRequest(erroValidacao);

                if (!string.IsNullOrEmpty(clienteAtualizacao.Email) && _repository.EmailJaCadastrado(cliente.Cpf, clienteAtualizacao.Email))
                    return BadRequest("Email já cadastrado.");

                if (!string.IsNullOrEmpty(clienteAtualizacao.Email))
                    cliente.Email = clienteAtualizacao.Email;
                if (clienteAtualizacao.RendimentoAnual.HasValue)
                    cliente.RendimentoAnual = clienteAtualizacao.RendimentoAnual.Value;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Endereco))
                    cliente.Endereco = clienteAtualizacao.Endereco;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Cidade))
                    cliente.Cidade = clienteAtualizacao.Cidade;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Estado))
                    cliente.Estado = clienteAtualizacao.Estado;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Cep))
                    cliente.Cep = clienteAtualizacao.Cep;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Ddd))
                    cliente.Ddd = clienteAtualizacao.Ddd;
                if (!string.IsNullOrEmpty(clienteAtualizacao.Telefone))
                    cliente.Telefone = clienteAtualizacao.Telefone;

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
    }
}
