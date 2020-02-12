using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Classes
{
    public partial class Contato : IDisposable
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

        public int Codigo
        {
            get { return _codigo; }
            set 
            {
                if (value < 0)
                {
                    // Na definição do método ou local que pode ocorrer o erro, lanço a exceção 
                    // Para que ela seja capturada em algum lugar, onde ele for usado.
                    throw new Loja.Excecoes.ValidacaoException("Código Contato deve ser inteiro positivo.");
                    _codigo = 0;
                }

                _isModified = true;
                _codigo = value; 
            }
        }

        private string _dadosContato;

        public string DadosContato
        {
            get { return _dadosContato; }
            set { _isModified = true; _dadosContato = value; }
        }

        private string _tipo;

        public string Tipo
        {
            get { return _tipo; }
            set { _isModified = true; _tipo = value; }
        }

        private int _codigoCliente;

        public int CodigoCliente
        {
            get { return _codigoCliente; }
            set { _codigoCliente = value; }
        }

    }
}
