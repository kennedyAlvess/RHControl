using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace RHControl.Repository
{
    public class UsuarioRepository
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;

        public bool AutenticarUsuario(string login, string senha)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT SenhaHash, Salt FROM Usuarios WHERE Login = :Login";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("@Login", login));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string senhaHashArmazenada = reader["SenhaHash"].ToString();
                                string salt = reader["Salt"].ToString();

                                string senhaHashFornecida = GerarHashSenha(senha, salt);

                                return senhaHashArmazenada == senhaHashFornecida;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);

                }
                return false;
            }
        }

        private string GerarHashSenha(string senha, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha + salt);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}