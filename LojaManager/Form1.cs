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
        BindingSource dadosBindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();

            dataGridViewClientes.AllowUserToAddRows = false;
            dataGridViewClientes.AllowUserToDeleteRows = false;

            // TODO: não vi a implementação disso nos videos. Estudar:
            dataGridViewClientes.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Puxando os dados do banco (via Cliente.TodosClientes()) para uma lista...
            dadosBindingSource.DataSource = new BindingList<Cliente>(Cliente.TodosClientes());
            // ... e carregando o grid com os dados da lista.
            dataGridViewClientes.DataSource = dadosBindingSource;

            txtCodigo.DataBindings.Add("Text", dadosBindingSource, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("Text", dadosBindingSource, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTipo.DataBindings.Add("Text", dadosBindingSource, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            dtCadastro.DataBindings.Add("Text", dadosBindingSource, "DataCadastro", true, DataSourceUpdateMode.OnPropertyChanged);

            // Dizendo que a fonte de dados para esse campo do grid é a lista de contados do cliente corrente, 
            // ou seja, que for selecionado:  ((Cliente)dadosBindingSource.Current).Contatos
            //TODO: formulário ainda não recebe os dados conforme seleção do Grid de Contatos.
            txtCodigoContato.DataBindings.Add("Text", ((Cliente)dadosBindingSource.Current).Contatos, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtClienteReferente.DataBindings.Add("Text", ((Cliente)dadosBindingSource.Current).Contatos, "CodigoCliente", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDadosContato.DataBindings.Add("Text", ((Cliente)dadosBindingSource.Current).Contatos, "DadosContato", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTipoDadoContato.DataBindings.Add("Text", ((Cliente)dadosBindingSource.Current).Contatos, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);

            // Um handler de evento para quando alterar um Cliente no grid de cima, provocar a alteração
            // do grid de baixo com os respectivos contatos.
            dadosBindingSource.CurrentChanged += onDadosBindingSource_CurrentChanged;
        }

        private void onDadosBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            // Definindo a mesma fonte de dados do grid superior (clientes) para o inferior (contatos).
            dataGridViewContatos.DataSource = ((Cliente)dadosBindingSource.Current).Contatos; 
        }

        private void Gravar_Click(object sender, EventArgs e)
        {
            ((Cliente)dadosBindingSource.Current).Gravar();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente excluir esse cliente?", "Confirme."
                                , MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                                == DialogResult.Yes)
            {
                // Remove o item corrente do Banco mas não remove da lista corrente.
                ((Cliente)dadosBindingSource.Current).Apagar();
                // Remove o item corrente da lista mas não remove do banco.
                dadosBindingSource.RemoveCurrent();
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            dadosBindingSource.Add(new Cliente());
            dadosBindingSource.MoveLast();
        }
    }
}
