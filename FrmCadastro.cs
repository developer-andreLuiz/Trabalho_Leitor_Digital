using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DPFP;

namespace LeitorDigital3
{
    public partial class FrmCadastro : Form
    {
        MySqlConexao mySql = new MySqlConexao();
        public FrmCadastro()
        {
            InitializeComponent();
        }
    
        private void EnrollmentControl1_OnEnroll(object Control, int FingerMask, DPFP.Template Template, ref DPFP.Gui.EventHandlerStatus EventHandlerStatus)
        {
            try
            {
               byte[] b = new byte[Template.Bytes.Length];
               Template.Serialize(ref b);
               mySql.InserirDados( b,txtNome.Text);
              
             
            }
            catch(Exception a )
            {
                MessageBox.Show(a.Message);
            }
        }

        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            txtNome.Text = string.Empty;
        }
    }
}
