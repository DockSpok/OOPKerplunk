using System;
using System.Collections.Generic;
using System.Data.SqlClient;

// Esta classe está colocada fora do namespace para que possa ser acessada de qualquer parte do sistema
// Apenas ilustrando essa possibilidade. 
public static class MetodosExtensao
{
    public static double Metade(this int valor)
    {
        return Convert.ToDouble(valor) / 2;
    }
}

namespace Loja.Classes
{
    public partial class Cliente : IDisposable
    {
        public Cliente()
        {
            _codigo = Proximo();
            // Como esse construtor não recebe parâmetros qualquer instancia tem que ser valorada por si,
            // portanto, o novo objeto será Novo (_isNew = true)
            _isNew = true;
            // Modified só pode ser o oposto.
            _isModified = false;
        }

        public Cliente(int codigo)
        {
            Ler(codigo);
            // Observer está no método
        }

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
                    throw new Exception("Não foi possível estabelecer a conexão com o Banco de Dados.", e);
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"Insert Into Cliente (Codigo, Nome, Tipo, DataCadastro)
                                                Values (@codigo, @nome, @tipo, @dataCadastro)";
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.AddWithValue("@codigo", _codigo);
                    sqlCommand.Parameters.AddWithValue("@nome", _nome);
                    sqlCommand.Parameters.AddWithValue("@tipo", _tipo);
                    // TODO: bug se usuário não altera a data atual, não popula o grid.
                    sqlCommand.Parameters.AddWithValue("@dataCadastro", _dataCadastro);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        // A partir desse momento, se não lançou uma exceção é porque já persistiu, portano
                        // o estado já deve mudar para 'não-Novo':
                        _isNew = false;
                    }
                    catch (Exception)
                    {
                        throw;
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
                    throw new Exception("Não foi possível estabelecer a conexão com o Banco de Dados.", e);
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandText = @"Update Cliente Set 
                                                  Nome = @nome
                                                , Tipo = @tipo
                                                , DataCadastro = @dataCadastro 
                                                Where Codigo = @codigo";
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.AddWithValue("@codigo", _codigo);
                    sqlCommand.Parameters.AddWithValue("@nome", _nome);
                    sqlCommand.Parameters.AddWithValue("@tipo", _tipo);
                    sqlCommand.Parameters.AddWithValue("@dataCadastro", _dataCadastro);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        // A partir do momento, se não lançou uma exceção é porque já persistiu, portano
                        // o estado já deve mudar para não modificado:
                        _isModified = false;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }

        public void Gravar()
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
                    sqlCommand.CommandText = @"Delete * From Cliente Where Codigo = @codigo";
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

        public void Ler(int codigoCliente)
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
                    sqlCommand.CommandText = @"Select * From Cliente Where Codigo = @codigo";
                    sqlCommand.Parameters.AddWithValue("@codigo", codigoCliente);

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            sqlDataReader.Read();

                            _codigo = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Codigo"));
                            _nome = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Nome"));
                            _tipo = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Tipo"));
                            _dataCadastro = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("DataCadastro"));

                            //_contatos = Contato.TodosContatos(_codigo);
                        }
                    }

                    _isNew = false;
                    _isModified = false;
                }
            }
        }

        // Métodos Estáticos são usados para que possam ser chamados 
        // sem a necessidade de se instanciar a classe a que pertencem.
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
                    sqlCommand.CommandText = @"Select Max(Codigo) + 1 From Cliente";

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

        public static List<Cliente> TodosClientes()
        {
            List<Cliente> _resultado = new List<Cliente>();

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
                    sqlCommand.CommandText = @"Select * From Cliente";

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                Cliente cliente = new Cliente();
                                cliente._codigo = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Codigo"));
                                cliente._nome = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Nome"));
                                cliente._tipo = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Tipo"));
                                cliente._dataCadastro = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("DataCadastro"));

                                cliente._contatos = Contato.TodosContatos(cliente._codigo);

                                cliente._isNew = false;

                                _resultado.Add(cliente);
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
