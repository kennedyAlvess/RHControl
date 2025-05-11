using Oracle.ManagedDataAccess.Client;
using RHControl.DTO.Pessoa;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace RHControl.Repository
{
    public class PessoaSalarioRepository
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;
        public void Remove(int requestId)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var cargos = new List<Cargo>();

                    string query = @"DELETE FROM PESSOA_SALARIO
                                     WHERE PESSOA_ID = :requestId";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("@requestId", requestId));
                        command.ExecuteNonQuery();
                    }
                }
                catch (OracleException)
                {
                    throw new ArgumentException("Erro ao executar comando.");
                }
                catch (Exception)
                {
                    throw new ArgumentException("Erro inesperado.");
                }
            }
        }
    }
}
