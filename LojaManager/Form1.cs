using Loja.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LojaManager
{
    public partial class Form1 : Form
    {
        BindingSource dados = new BindingSource();
        public Form1()
        {
            InitializeComponent();

            dataGridViewClientes.AllowUserToAddRows = false;
            dataGridViewClientes.AllowUserToDeleteRows = false;
            // TODO: não vi a implementação disso nos videos. Estudar:
            dataGridViewClientes.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Puxando os dados do banco (via Cliente.TodosClientes()) para uma lista...
            //dados.DataSource = new BindingList<Cliente>(Cliente.TodosClientes());
            dados.DataSource = Cliente.TodosClientes();
            // ... e carregando o grid com os dados da lista.
            dataGridViewClientes.DataSource = dados;

            txtCodigo.DataBindings.Add("Text", dados, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("Text", dados, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTipo.DataBindings.Add("Text", dados, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            dtCadastro.DataBindings.Add("Text", dados, "DataCadastro", true, DataSourceUpdateMode.OnPropertyChanged);

            dataGridViewContatos.AllowUserToAddRows = false;
            dataGridViewContatos.AllowUserToDeleteRows = false;
            dataGridViewContatos.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewContatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Dizendo que a fonte de dados para esses campos de dados é a lista de contados do cliente corrente, 
            // ou seja, o cliente que estiver selecionado:  ((Cliente)dados.Current).Contatos
            //TODO: formulário ainda não recebe os dados conforme seleção do Grid de Contatos.
            txtCodigoContato.DataBindings.Add("Text", ((Cliente)dados.Current).Contatos, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtClienteReferente.DataBindings.Add("Text", ((Cliente)dados.Current).Contatos, "CodigoCliente", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDadosContato.DataBindings.Add("Text", ((Cliente)dados.Current).Contatos, "DadosContato", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTipoDadoContato.DataBindings.Add("Text", ((Cliente)dados.Current).Contatos, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            // Um handler de evento para quando alterar um Cliente no grid de cima, provocar a alteração
            // do grid de baixo com os respectivos contatos.
            dados.CurrentChanged += onDadosBindingSource_CurrentChanged;
            // A linha abaixo é uma repetição da ação do evento, está aqui para garantir 
            // que os contatos sejam carregados mesmo sem um clique em um Cliente no grid.
            dataGridViewContatos.DataSource = ((Cliente)dados.Current).Contatos;
        }

        private void onDadosBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            // Definindo a mesma fonte de dados do grid superior (clientes) para o inferior (contatos).
            dataGridViewContatos.DataSource = ((Cliente)dados.Current).Contatos; 
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            ((Cliente)dados.Current).Gravar();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir esse cliente?", "Confirme."
                                , MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                                == DialogResult.Yes)
            {
                // Remove o item corrente do Banco mas não remove da lista corrente.
                ((Cliente)dados.Current).Apagar();
                // Remove o item corrente da lista mas não remove do banco.
                dados.RemoveCurrent();
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            dados.Add(new Cliente());
            dados.MoveLast();
        }
    }
}
