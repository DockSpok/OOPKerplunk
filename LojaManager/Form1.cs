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
        BindingSource bindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            bindingSource.DataSource = Cliente.TodosClientes();

            dataGridView1.DataSource = bindingSource;

            txtCodigo.DataBindings.Add("Text", bindingSource, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("Text", bindingSource, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTipo.DataBindings.Add("Text", bindingSource, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            dtCadastro.DataBindings.Add("Text", bindingSource, "DataCadastro", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void Gravar_Click(object sender, EventArgs e)
        {
            ((Cliente)bindingSource.Current).Gravar();
        }
    }
}
