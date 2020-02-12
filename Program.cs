using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //using (Classes.Cliente clienteLeitura = new Classes.Cliente(3))
                //{
                //    // passando o parametro codigo logo no construtor e posso usar o objeto clienteLeitura 
                //    // que será descartado logo em seguida pelo IDispose
                //}
                
                using (Classes.Cliente clienteUpdate = new Classes.Cliente(3))
                {
                    // passando o parametro codigo logo no construtor e posso usar o objeto clienteLeitura 
                    // que será descartado logo em seguida pelo IDispose
                    clienteUpdate.Nome = "NovoAbcd"; // Nome com menos de 3 caracteres lança exceção. 
                }

                //Classes.Cliente clienteA = new Classes.Cliente();
                ////clienteA.Codigo = 3; // Codigo negativo lança exceção.
                //clienteA.Nome = "Nome com PK automatica"; // Nome com menos de 3 caracteres lança exceção. 
                //clienteA.Tipo = 2;
                //clienteA.DataCadastro = new DateTime(2020, 5, 1);

                //clienteA.Contatos = new List<Classes.Contato>();
                //clienteA.Contatos.Add(contato1);
                //clienteA.Contatos.Add(contato2);

                //clienteA.Gravar();

                //Classes.Contato contatoBuscado = clienteA.Contatos.FirstOrDefault(x => x.Tipo == "Telefone");

                //Console.WriteLine($"Dados encontrados para o cliente {0}: {1}", clienteA.Nome, contatoBuscado);

                // Qualquer exceção que for lançada por um erro dentro do Try será
            }
            // Capturada e passada como parâmetro do catch
            catch (Loja.Excecoes.ValidacaoException eita)
            {
                // O throw serve para enviar a exceção cada vez mais para uma camada mais externa.
                // Nesse caso o programa quebra porque é a última camada. 
                // Para usar os métodos extendidos da classe de exceções
                // Inseri um parâmetro 'eita' no método catch (Loja.Excecoes.ValidacaoException)
                Console.WriteLine(eita.HelpLink);
                throw;
            }
            // O bloco using garante que o objeto da classe que implementa 
            // IDisposable será descartado após o uso.
            //using (Classes.Cliente clienteB = new Classes.Cliente())
            //{
            //    clienteB.Nome = "Xyz";
            //    clienteB.Codigo = 5;
            //    double resultadoDaAplicacaoDoMetodoDeExtensao = clienteB.Codigo.Metade();
            //}
            // Se a chamada ocorre nesse ponto, as demais propriedades do objeto não serão populadas.
            // A solução é simplesmente inserir a chamada linhas abaixo.
            //clienteA.Gravar();

            Classes.Contato contato1 = new Classes.Contato();
            contato1.Codigo = 1;
            contato1.Tipo = "Telefone";
            contato1.DadosContato = "5555-5555";

            Classes.Contato contato2 = new Classes.Contato();
            contato1.Codigo = 2;
            contato2.Tipo = "email";
            contato2.DadosContato = "armando@mail";

        }
    }
}
