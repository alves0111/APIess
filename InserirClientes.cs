using System;
using System.Collections.Generic;
using Npgsql;

namespace ApiToPostgres
{
    class Program
    {
        static void Main(string[] args)
        {
            // Sua string de conexão
            string connectionString = "Host=localhost;Port=5432;Username=seu_usuario;Password=sua_senha;Database=seu_banco";

            // Exemplo: lista de clientes simulada
            List<ClientDto> clientes = ObterClientesDaApi(); // aqui você chama sua API

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                foreach (var cliente in clientes)
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO clientes (cod_cli, nome) VALUES (@cod_cli, @nome)", conn))
                    {
                        cmd.Parameters.AddWithValue("@cod_cli", (object)cliente.COD_CLI ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@nome", (object)cliente.NOME ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            Console.WriteLine("Dados inseridos com sucesso!");
        }

        // Aqui você cria seu DTO e método de API
        static List<ClientDto> ObterClientesDaApi()
        {
            return new List<ClientDto>
            {
                new ClientDto { COD_CLI = "001", NOME = "Teste Cliente" }
            };
        }
    }

    public class ClientDto
    {
        public string COD_CLI { get; set; }
        public string NOME { get; set; }
        // Adicione os outros campos conforme necessário
    }
}