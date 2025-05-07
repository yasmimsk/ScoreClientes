using Microsoft.AspNetCore.Mvc;
using Moq;
using ScoreClientes.API.Controllers;
using ScoreClientes.API.Interfaces;
using ScoreClientes.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreClientes.Tests
{
    public class ClienteControllerTests
    {
        private readonly Mock<IClienteRepository> _mockRepository;
        private readonly Mock<IClienteService> _mockService;
        private readonly ClienteController _controller;

        public ClienteControllerTests()
        {
            _mockRepository = new Mock<IClienteRepository>();
            _mockService = new Mock<IClienteService>();
            _controller = new ClienteController(_mockRepository.Object, _mockService.Object);
        }

        [Fact]
        public void CadastrarCliente_DeveRetornarOk_QuandoDadosValidos()
        {
            var cliente = new Cliente
            {
                Nome = "Teste",
                DataNascimento = DateTime.Today.AddYears(-30),
                Cpf = "12345678909",
                Email = "teste@teste.com",
                RendimentoAnual = 100000,
                Endereco = "Rua xyz",
                Cidade = "Curitiba",
                Estado = "PR",
                Cep = "12345678",
                Ddd = "41",
                Telefone = "987654321"
            };

            _mockRepository.Setup(r => r.ObterPorCpf(cliente.Cpf)).Returns((Cliente)null);
            _mockRepository.Setup(r => r.ObterPorEmail(cliente.Email)).Returns((Cliente)null);

            var result = _controller.CadastrarCliente(cliente);
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CadastrarCliente_DeveRetornarBadRequest_QuandoCpfInvalido()
        {
            var cliente = new Cliente
            {
                Nome = "Teste",
                DataNascimento = DateTime.Today.AddYears(-30),
                Cpf = "12345678900",
                Email = "teste@teste.com",
                RendimentoAnual = 100000,
                Endereco = "Rua xyz",
                Cidade = "Curitiba",
                Estado = "PR",
                Cep = "12345678",
                Ddd = "41",
                Telefone = "987654321"
            };

            var result = _controller.CadastrarCliente(cliente);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CadastrarCliente_DeveRetornarBadRequest_QuandoEmailInvalido()
        {
            var cliente = new Cliente
            {
                Nome = "Teste",
                DataNascimento = DateTime.Today.AddYears(-30),
                Cpf = "12345678909",
                Email = "teste",
                RendimentoAnual = 100000,
                Endereco = "Rua xyz",
                Cidade = "Curitiba",
                Estado = "PR",
                Cep = "12345678",
                Ddd = "41",
                Telefone = "987654321"
            };

            var result = _controller.CadastrarCliente(cliente);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CadastrarCliente_DeveRetornarBadRequest_QuandoCpfJaExiste()
        {
            var cliente = new Cliente
            {
                Nome = "Teste",
                DataNascimento = DateTime.Today.AddYears(-30),
                Cpf = "12345678909",
                Email = "teste@teste.com",
                RendimentoAnual = 100000,
                Endereco = "Rua xyz",
                Cidade = "Curitiba",
                Estado = "PR",
                Cep = "12345678",
                Ddd = "41",
                Telefone = "987654321"
            };

            _mockRepository.Setup(r => r.ObterPorCpf(cliente.Cpf)).Returns(new Cliente());

            var result = _controller.CadastrarCliente(cliente);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ObterTodos_DeveRetornarListaDeClientes()
        {
            var clientes = new List<Cliente> { new Cliente(), new Cliente() };
            _mockRepository.Setup(r => r.ObterTodos()).Returns(clientes);

            var result = _controller.ObterTodos();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IList<Cliente>>(okResult.Value);
            Assert.Equal(2, retorno.Count);
        }

        [Fact]
        public void ObterPorId_DeveRetornarCliente_QuandoEncontrado()
        {
            var cliente = new Cliente();
            _mockRepository.Setup(r => r.ObterPorId(1)).Returns(cliente);

            var result = _controller.ObterPorId(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cliente, okResult.Value);
        }

        [Fact]
        public void ObterPorId_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            _mockRepository.Setup(r => r.ObterPorId(1)).Returns((Cliente)null);

            var result = _controller.ObterPorEmail("teste@teste.com");
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ObterPorCpf_DeveRetornarCliente_QuandoEncontrado()
        {
            var cliente = new Cliente();
            _mockRepository.Setup(r => r.ObterPorCpf("12345678909")).Returns(cliente);

            var result = _controller.ObterPorCpf("12345678909");
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cliente, okResult.Value);
        }

        [Fact]
        public void ObterPorCpf_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            _mockRepository.Setup(r => r.ObterPorCpf("12345678909")).Returns((Cliente)null);

            var result = _controller.ObterPorCpf("12345678909");
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ObterPorEmail_DeveRetornarCliente_QuandoEncontrado()
        {
            var cliente = new Cliente();
            _mockRepository.Setup(r => r.ObterPorEmail("teste@teste.com")).Returns(cliente);

            var result = _controller.ObterPorEmail("teste@teste.com");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cliente, okResult.Value);
        }

        [Fact]
        public void ObterPorEmail_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            _mockRepository.Setup(r => r.ObterPorEmail("teste@teste.com")).Returns((Cliente)null);

            var result = _controller.ObterPorEmail("teste@teste.com");
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ObterPorEstado_DeveRetornarListaDeClientes()
        {
            var clientes = new List<Cliente> { new Cliente(), new Cliente() };
            _mockRepository.Setup(r => r.ObterPorEstado("PR")).Returns(clientes);

            var result = _controller.ObterPorEstado("PR");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IList<Cliente>>(okResult.Value);
            Assert.Equal(2, retorno.Count);
        }

        [Fact]
        public void AtualizarCliente_DeveAtualizarEmail_QuandoValido()
        {
            var cliente = new Cliente { Email = "antigo@email.com" };
            _mockRepository.Setup(r => r.ObterPorId(1)).Returns(cliente);

            var novo = new ClienteAtualizacao { Email = "novo@email.com" };

            var result = _controller.AtualizarCliente(1, novo);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("novo@email.com", cliente.Email);
        }

        [Fact]
        public void AtualizarCliente_DeveRetornarNotFound_QuandoClienteNaoExiste()
        {
            _mockRepository.Setup(r => r.ObterPorId(1)).Returns((Cliente)null);

            var result = _controller.AtualizarCliente(1, new ClienteAtualizacao());

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AtualizarCliente_DeveRetornarBadRequest_QuandoEmailInvalido()
        {
            var cliente = new Cliente { Email = "antigo@email.com" };
            _mockRepository.Setup(r => r.ObterPorId(1)).Returns(cliente);

            var update = new ClienteAtualizacao { Email = "novo" };

            var result = _controller.AtualizarCliente(1, update);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeletarCliente_DeveRetornarOk_QuandoClienteExiste()
        {
            var cliente = new Cliente();
            _mockRepository.Setup(r => r.ObterPorId(1)).Returns(cliente);

            var result = _controller.DeletarCliente(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeletarCliente_DeveRetornarNotFound_QuandoClienteNaoExiste()
        {
            var cliente = new Cliente();
            _mockRepository.Setup(r => r.ObterPorId(1)).Returns((Cliente)null);

            var result = _controller.DeletarCliente(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ObterScore_DeveRetornarScore_QuandoClienteExiste()
        {
            var cliente = new Cliente { Cpf = "12345678909" };
            _mockRepository.Setup(r => r.ObterPorCpf(cliente.Cpf)).Returns(cliente);
            _mockService.Setup(s => s.CalcularScore(cliente)).Returns(500);

            var result = _controller.ObterScore(cliente.Cpf);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(500, okResult.Value);
        }

        [Fact]
        public void ObterScore_DeveRetornarNotFound_QuandoClienteNaoExiste()
        {
            _mockRepository.Setup(r => r.ObterPorCpf("12345678909")).Returns((Cliente)null);

            var result = _controller.ObterScore("12345678909");

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
