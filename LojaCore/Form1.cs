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

namespace LojaCore
{
    public partial class Form1 : Form
    {
        BindingSource bindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            DataGridView dataGridView = new DataGridView();
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;

            bindingSource.DataSource = Cliente.TodosClientes();

            dataGridView.DataSource = bindingSource;

        }

    }
}
