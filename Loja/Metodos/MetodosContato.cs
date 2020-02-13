using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Classes
{
    public partial class Contato
    {

        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HOME\source\repos\OOPBasicaoParte01\LojaDb.mdf;Integrated Security=True";

        public void Insert()
        {
            // Com o método 'using' garanto que vou descartar após o uso.
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception e)
                {
                    throw;
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"Insert Into Contato (Codigo, DadosContato, Tipo)
                                                Values (@codigo, @dadosContato, @tipo)";
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.AddWithValue("@codigo", _codigo);
                    sqlCommand.Parameters.AddWithValue("@dadosContato", _dadosContato);
                    sqlCommand.Parameters.AddWithValue("@tipo", _tipo);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        // A partir do momento, se não lançou uma exceção é porque já persistiu, portano
                        // o estado já deve mudar para não Novo:
                        _isNew = false;
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Não foi possível Inserir o Contato {_codigo.ToString()}", e);
                    }

                }
            }
        }

        public void Update()
        {
            // Com o método 'using' garanto que vou descartar após o uso.
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception e)
                {
                    throw new Excecoes.ValidacaoException("Não foi possível abrir a conexão para Contatos.", e);
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"Update Contato Set 
                                                  DadosContato = @dadosContato
                                                , Tipo = @tipo 
                                                Where Codigo = @codigo";
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.AddWithValue("@codigo", _codigo);
                    sqlCommand.Parameters.AddWithValue("@dadosContato", _dadosContato);
                    sqlCommand.Parameters.AddWithValue("@tipo", _tipo);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        // A partir do momento, se não lançou uma exceção é porque já persistiu, portano
                        // o estado já deve mudar para 'não-modificado':
                        _isModified = false;
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Não foi possível Atualizar o Contato {_codigo.ToString()}", e);
                    }

                }
            }
        }

        void Gravar()
        {
            if (_isNew)
            {
                Insert();
            }
            else if (_isModified)
            {
                Update();
            }
        }

        public void Apagar()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception e)
                {
                    throw;
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"Delete * From Contato Where Codigo = @codigo";
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.AddWithValue("@codigo", _codigo);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public void Ler(int codigoContato)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception)
                {
                    throw new Loja.Excecoes.ValidacaoException("Não foi possível estabelecer conexão com o Banco de Dados.");
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = @"Select * From Contato Where Codigo = @codigo";
                    sqlCommand.Parameters.AddWithValue("@codigo", codigoContato);

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            sqlDataReader.Read();

                            _codigo = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Codigo"));
                            _dadosContato = sqlDataReader.GetString(sqlDataReader.GetOrdinal("DadosContato"));
                            _tipo = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Tipo"));
                            _codigoCliente = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("CodigoCliente"));
                        }
                    }

                    _isNew = false;
                    _isModified = false;
                }
            }
        }

        public static Int32 Proximo()
        {
            Int32 _resultado = 0;

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception)
                {
                    throw new Loja.Excecoes.ValidacaoException("Não foi possível estabelecer conexão com o Banco de Dados.");
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = @"Select Max(Codigo) + 1 From Contato";

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            sqlDataReader.Read();
                            // o Reader só vai ter uma linha nesse momento...
                            _resultado = sqlDataReader.GetInt32(0);
                        }
                    }
                }
            }

            return _resultado;
        }

        public static List<Contato> TodosContatos(int codigoCliente)
        {
            List<Contato> _resultado = null; // Porque um cliente pode não ter contado algum.

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                }
                catch (Exception e)
                {
                    throw new Loja.Excecoes.ValidacaoException("Não foi possível estabelecer conexão com o Banco de Dados.", e);
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = @"Select * From Contato Where CodigoCliente = @codigoCliente";
                    sqlCommand.Parameters.AddWithValue("@codigoCliente", codigoCliente);

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                Contato contato = new Contato();
                                contato._codigo = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Codigo"));
                                contato._dadosContato = sqlDataReader.GetString(sqlDataReader.GetOrdinal("DadosContato"));
                                contato._tipo = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Tipo"));
                                contato._codigoCliente = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("CodigoCliente"));

                                if (_resultado == null)
                                {
                                    _resultado = new List<Contato>();
                                }

                                contato._isNew = false;

                                _resultado.Add(contato);
                            }
                        }
                    }
                }
            }

            return _resultado;
        }

        public void Dispose()
        {
            this.Gravar();
        }


    }
}
