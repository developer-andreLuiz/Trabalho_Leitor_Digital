using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeitorDigital3
{
    public partial class FrmResgistro : Form
    {
        MySqlConexao conexao = new MySqlConexao();
        public FrmResgistro()
        {
            InitializeComponent();
        }

        private void FrmResgistro_Load(object sender, EventArgs e)
        {
            conexao.CarregarDataGrid(dataGridView1);
        }
    }
}
