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


        public void Dispose()
        {
            this.Gravar();
        }


    }
}
