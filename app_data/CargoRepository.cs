using Oracle.ManagedDataAccess.Client;
using RHControl.DTO.Pessoa;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace RHControl.Repository
{
    public class CargoRepository
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;
        public List<Cargo> GetCargos()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var cargos = new List<Cargo>();

                    string query = @"SELECT c.ID, c.Nome CARGO 
                                    FROM CARGO c 
                                    order by c.Nome";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cargos.Add(
                                    new Cargo()
                                    {
                                        Id = Convert.ToInt32(reader["ID"]),
                                        Nome = reader["CARGO"].ToString()
                                    });
                            }
                        }
                    }
                    return cargos;
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
