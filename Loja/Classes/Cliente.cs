using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Classes
{
    public partial class Cliente : IDisposable
    {
        #region Observer
        private bool _isNew;

        [Browsable(false)] // Para não aparecer no GridView
        public bool IsNew
        {
            get { return _isNew; }
        }

        private bool _isModified;

        [Browsable(false)] // Para não aparecer no GridView
        public bool IsModified
        {
            get { return _isModified; }
        }
        #endregion observer

        private int _codigo;
        [DisplayName("Código")]
        public int Codigo
        {
            get { return _codigo; }
            set
            {
                if (value < 0)
                {
                    // Na definição do método ou local que pode ocorrer o erro, lanço a exceção 
                    // Para que ela seja capturada em algum lugar, onde ele for usado.
                    throw new Loja.Excecoes.ValidacaoException("Codigo Cliente deve ser inteiro positivo.");
                    _codigo = 0;
                }

                _isModified = true;
                _codigo = value;
            }
        }

        private string _nome;
        [DisplayName("Nome do Cliente")]
        public string Nome
        {
            get { return _nome; }
            set
            {
                if (value.Length < 3)
                {
                    // Apenas um exemplo de filtro.
                    //Loja.Excecoes.ValidacaoException("Nome não pode ter menos que três caracteres.");
                }

                _isModified = true;
                _nome = value;
            }
        }

        private int _tipo;

        public int Tipo
        {
            get { return _tipo; }
            set { _tipo = value; _isModified = true; }
        }

        // Usamos propfull para criar variável privada e propriedade ao mesmo tempo.
        private DateTime _dataCadastro = DateTime.MinValue;
        [DisplayName("Data do Cadastro")]
        public DateTime DataCadastro
        {
            get { return _dataCadastro; }
            set { 
                    _dataCadastro = value > DateTime.MinValue ? value : DateTime.MinValue; 
                    _isModified = true; 
                }
        }

        private List<Contato> _contatos;
        public List<Contato> Contatos
        {
            get { return _contatos; }
            set { _contatos = value; }
        }

        // kerplunk havia deixado esse método nessa parcial da classe. Passei para MetodosCliente
        //public void Dispose()
        //{
        //    this.Gravar();
        //}
    }
}
