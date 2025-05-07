using Microsoft.Data.SqlClient;
using ScoreClientes.API.Interfaces;
using ScoreClientes.API.Models;

namespace ScoreClientes.API.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;

        public ClienteRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public void InserirCliente(Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Cliente
                            (Nome, DataNascimento, Cpf, Email, RendimentoAnual, Endereco, Cidade, Estado, Cep, Ddd, Telefone)
                            VALUES
                            (@Nome, @DataNascimento, @Cpf, @Email, @RendimentoAnual, @Endereco, @Cidade, @Estado, @Cep, @Ddd, @Telefone)";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
                command.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@RendimentoAnual", cliente.RendimentoAnual);
                command.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                command.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                command.Parameters.AddWithValue("@Estado", cliente.Estado);
                command.Parameters.AddWithValue("@Cep", cliente.Cep);
                command.Parameters.AddWithValue("@Ddd", cliente.Ddd);
                command.Parameters.AddWithValue("@Telefone", cliente.Telefone);

                command.ExecuteNonQuery();
            }
        }

        public IList<Cliente> ObterTodos()
        {
            IList<Cliente> clientes = new List<Cliente>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Cliente ORDER BY Nome", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(PreencherCliente(reader));
                }
            }

            return clientes;
        }

        public Cliente ObterPorId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Cliente WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                var reader = command.ExecuteReader();
                if (reader.Read())
                    return PreencherCliente(reader);
            }

            return null;
        }

        public Cliente ObterPorCpf(string cpf)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Cliente WHERE Cpf = @Cpf", connection);
                command.Parameters.AddWithValue("@Cpf", cpf);

                var reader = command.ExecuteReader();
                if (reader.Read())
                    return PreencherCliente(reader);
            }

            return null;
        }

        public Cliente ObterPorEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Cliente WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                var reader = command.ExecuteReader();
                if (reader.Read())
                    return PreencherCliente(reader);
            }

            return null;
        }

        public IList<Cliente> ObterPorEstado(string estado)
        {
            IList<Cliente> clientes = new List<Cliente>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT * FROM Cliente WHERE Estado = @Estado ORDER BY Nome", connection);
                command.Parameters.AddWithValue("@Estado", estado);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(PreencherCliente(reader));
                }
            }

            return clientes;
        }

        public void Atualizar(int id, Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE Cliente
                            SET Email = @Email,
                                RendimentoAnual = @RendimentoAnual,
                                Endereco = @Endereco,
                                Cidade = @Cidade,
                                Estado = @Estado,
                                Cep = @Cep,
                                Ddd = @Ddd,
                                Telefone = @Telefone
                            WHERE Id = @Id";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@RendimentoAnual", cliente.RendimentoAnual);
                command.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                command.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                command.Parameters.AddWithValue("@Estado", cliente.Estado);
                command.Parameters.AddWithValue("@Cep", cliente.Cep);
                command.Parameters.AddWithValue("@Ddd", cliente.Ddd);
                command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        public void Deletar(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM Cliente WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        private Cliente PreencherCliente(SqlDataReader reader)
        {
            return new Cliente
            {
                Nome = reader["Nome"].ToString(),
                DataNascimento = DateTime.Parse(reader["DataNascimento"].ToString()),
                Cpf = reader["Cpf"].ToString(),
                Email = reader["Email"].ToString(),
                RendimentoAnual = Convert.ToDecimal(reader["RendimentoAnual"]),
                Endereco = reader["Endereco"].ToString(),
                Cidade = reader["Cidade"].ToString(),
                Estado = reader["Estado"].ToString(),
                Cep = reader["Cep"].ToString(),
                Ddd = reader["Ddd"].ToString(),
                Telefone = reader["Telefone"].ToString()
            };
        }
    }
}
