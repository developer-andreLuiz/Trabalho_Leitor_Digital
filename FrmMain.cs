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
    public partial class FrmMain : Form
    {
        int valor = 0;
        public FrmMain()
        {
            InitializeComponent();
        }

     
        private void Timer_Tick(object sender, EventArgs e)
        {
            valor++;
            if (valor ==2 )
            {
                //this.Hide();
                this.Size = new Size(0, 0);
                FrmVerificar f = new FrmVerificar();
                f.Closed += (s, args) => this.Close();
                f.Show();
                timer.Enabled = false;
            }
        }
    }
}
