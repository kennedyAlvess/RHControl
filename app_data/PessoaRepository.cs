using Oracle.ManagedDataAccess.Client;
using RHControl.DTO.Pessoa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RHControl.Repository
{
    public class PessoaRepository
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;

        public List<PessoaList> GetPessoas(string requestNome, int cargoId, int cargoId2)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    List<PessoaList> pessoasList = new List<PessoaList>();
                    string query = @"SELECT p.ID, 
                                            p.NOME,
                                            c.Nome CARGO, 
                                            ps.SALARIO_LIQUIDO SALARIO 
                                    FROM PESSOA p 
                                    INNER JOIN CARGO c on c.ID = p.CARGO_ID 
                                    LEFT JOIN PESSOA_SALARIO ps on ps.PESSOA_ID = p.ID       
                                    WHERE ( :cargoId = 0 OR p.CARGO_ID = :cargoId2 )
                                       AND UPPER(p.NOME) like :requestNome || '%'
                                    ORDER BY p.NOME";

                    connection.Open();
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("cargoId", cargoId));
                        command.Parameters.Add(new OracleParameter("cargoId2", cargoId2));
                        command.Parameters.Add(new OracleParameter("requestNome", requestNome));
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pessoasList.Add(
                                    new PessoaList()
                                    {
                                        Id = Convert.ToInt32(reader["ID"]),
                                        Nome = reader["NOME"].ToString(),
                                        Cargo = reader["CARGO"].ToString(),
                                        Salario = reader.IsDBNull(reader.GetOrdinal("SALARIO")) ? 0 : Convert.ToDecimal(reader["SALARIO"])
                                    });
                            }
                        }
                    }
                    return pessoasList;
                }
                catch (OracleException ex)
                {
                    throw new ArgumentException("Erro ao executar comando. " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Erro inesperado. " + ex.Message);
                }
            }
        }
        public PessoaModel GetPessoaById(int requestId)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    PessoaModel pessoa = new PessoaModel();
                    string query = @"SELECT * FROM PESSOA WHERE ID = :Id";

                    connection.Open();
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("Id", requestId));
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PessoaModel()
                                {
                                    Id = Convert.ToInt32(reader["ID"]),
                                    Nome = reader["NOME"].ToString(),
                                    Cidade = reader["CIDADE"].ToString(),
                                    Email = reader["EMAIL"].ToString(),
                                    CEP = reader["CEP"].ToString(),
                                    Endereco = reader["ENDERECO"].ToString(),
                                    Pais = reader["PAIS"].ToString(),
                                    Usuario = reader["USUARIO"].ToString(),
                                    Telefone = reader["TELEFONE"].ToString(),
                                    Data_Nascimento = reader["DATA_NASCIMENTO"].ToString(),
                                    Cargo_Id = Convert.ToInt32(reader["CARGO_ID"])
                                };
                            }
                        }
                    }
                    return pessoa;
                }
                catch (OracleException ex)
                {
                    throw new ArgumentException("Erro ao executar comando. " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Erro Inesperado. " + ex.Message);
                }
            }
        }
        public int InsertOrUpdate(PessoaModel requestPessoa)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    string query = "";
                    bool isInsert = requestPessoa.Id is null;

                    if (isInsert)
                    {
                        query = @"INSERT INTO PESSOA (NOME, CIDADE, EMAIL, CEP, ENDERECO, PAIS, USUARIO, TELEFONE, DATA_NASCIMENTO, CARGO_ID) 
                                  VALUES (:Nome, :Cidade, :Email, :Cep, :Endereco, :Pais, :Usuario, :Telefone, :Data_Nascimento, :Cargo_Id )";
                    }
                    else
                    {
                        query = @"UPDATE PESSOA 
                                    SET NOME = :Nome, CIDADE = :Cidade, EMAIL = :Email, CEP = :Cep, ENDERECO = :Endereco, PAIS = :Pais,
                                        USUARIO = :Usuario, TELEFONE = :Telefone, DATA_NASCIMENTO = :Data_Nascimento, CARGO_ID = :Cargo_Id 
                                  WHERE ID = :Id";
                    }

                    connection.Open();
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add("Nome", OracleDbType.Varchar2).Value = requestPessoa.Nome;
                        command.Parameters.Add("Cidade", OracleDbType.Varchar2).Value = requestPessoa.Cidade;
                        command.Parameters.Add("Email", OracleDbType.Varchar2).Value = requestPessoa.Email;
                        command.Parameters.Add("Cep", OracleDbType.Varchar2).Value = requestPessoa.CEP;
                        command.Parameters.Add("Endereco", OracleDbType.Varchar2).Value = requestPessoa.Endereco;
                        command.Parameters.Add("Pais", OracleDbType.Varchar2).Value = requestPessoa.Pais;
                        command.Parameters.Add("Usuario", OracleDbType.Varchar2).Value = requestPessoa.Usuario;

                        requestPessoa.Telefone = Regex.Replace(requestPessoa.Telefone, @"\D", "");
                        command.Parameters.Add("Telefone", OracleDbType.Varchar2).Value = requestPessoa.Telefone;

                        command.Parameters.Add("Data_Nascimento", OracleDbType.Date).Value = DateTime.ParseExact(requestPessoa.Data_Nascimento, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        command.Parameters.Add("Cargo_Id", OracleDbType.Int32).Value = requestPessoa.Cargo_Id;

                        if (!isInsert)
                        {
                            command.Parameters.Add("Id", OracleDbType.Int32).Value = requestPessoa.Id.Value;
                            return command.ExecuteNonQuery();
                        }

                        return command.ExecuteNonQuery();
                    }
                }
                catch (OracleException ex)
                {
                    throw new ArgumentException("Erro ao executar comando. " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Erro Inesperado. " + ex.Message);
                }
            }
        }
        public int Remove(int requestId)
        {
            PessoaSalarioRepository _pessoaSalarioRepository = new PessoaSalarioRepository();
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _pessoaSalarioRepository.Remove(requestId);
                        string query = @"DELETE FROM PESSOA WHERE ID = :Id";

                        using (OracleCommand command = new OracleCommand(query, connection))
                        {
                            command.Parameters.Add(new OracleParameter("Id", requestId));
                            int rowsAffected = command.ExecuteNonQuery();

                            transaction.Commit();
                            return rowsAffected;
                        }
                    }
                    catch (OracleException ex)
                    {
                        transaction.Rollback();
                        throw new ArgumentException("Erro ao executar comando. " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new ArgumentException("Erro Inesperado. " + ex.Message);
                    }
                }
            }
        }
        public async Task<bool> CalcularSalarioPessoa(int requestId)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string sql = "PREENCHER_SALARIOS";
                    using (OracleCommand command = new OracleCommand(sql, connection))
                    {
                        command.Parameters.Add(new OracleParameter("requestId", OracleDbType.Int32)
                        {
                            Direction = System.Data.ParameterDirection.Input,
                            Value = requestId
                        });
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await command.ExecuteNonQueryAsync();
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public async Task<bool> CalcularSalarios()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string sql = "PREENCHER_SALARIOS";
                    using (OracleCommand command = new OracleCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new OracleParameter("requestId", OracleDbType.Int32)
                        {
                            Direction = System.Data.ParameterDirection.Input,
                            Value = DBNull.Value
                        });

                        await command.ExecuteNonQueryAsync();
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}