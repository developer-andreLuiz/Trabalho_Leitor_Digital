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
using DPFP.Capture;

namespace LeitorDigital3
{
    public partial class FrmVerificar : Form, DPFP.Capture.EventHandler
    {
        MySqlConexao conexao = new MySqlConexao();

        DPFP.Capture.SampleConversion sp = new DPFP.Capture.SampleConversion();
        DPFP.Capture.Capture cp = new DPFP.Capture.Capture();
        Bitmap img = null;
        int cont = 0;
        bool status = false;
        bool leituraPendente = false;
        bool cadastrado = false;
        string nome = null;
       
        
      
        public FrmVerificar()
        {
            InitializeComponent();
            cp.StartCapture();
            cp.EventHandler = this;
        }

        #region Form
        private void FrmVerificar_Activated(object sender, EventArgs e)
        {
            cp.StartCapture();
        }
        private void TimerVerificador_Tick(object sender, EventArgs e)
        {
            cont++;
            if (cont>0)
            {
                if (leituraPendente)
                {
                    if (cadastrado)
                    {
                        leituraPendente = false;
                        CadastroEncontrado();

                    }
                    else
                    {
                        leituraPendente = false;
                        CadastroNaoEncontrado();
                    }
                }
            }
          
        }
        #endregion

        #region Eventos Leitor
        public void OnComplete(object Capture, string ReaderSerialNumber, Sample Sample)
        {
            try
            {
                if (status==false)
                {
                    leituraPendente = true;
                    sp.ConvertToPicture(Sample, ref img);
                    pictureBox1.Image = img;
                    Process(Sample);
                }
               
            }
            catch (Exception a )
            {
                MessageBox.Show(a.Message);
            }
        }
        public void Process(DPFP.Sample Sample)
        {
            DPFP.FeatureSet featuresVerify = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);
            DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
            DPFP.Verification.Verification Verificator = new DPFP.Verification.Verification();

            DataTable dt = new DataTable();

            if (featuresVerify != null)
            {
                conexao.BuscarDadosLista(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    object objFPTemp = null;
                    object objID = null;
                    object objNome = null;
                    objFPTemp = dr["FPTemp"];
                    //objID = dr["EmployeelID"];
                    objNome = dr["Nome"];
                   

                    byte[] varbyte = (byte[])objFPTemp;

                    Template savedTemp = new Template();
                    savedTemp.DeSerialize(varbyte);

                    Verificator.Verify(featuresVerify, savedTemp, ref result);

                    if (result.Verified)
                    {
                        cont = 0;
                        cadastrado = true;
                        nome = objNome.ToString();
                      
                        conexao.ObjVazio();
                        conexao.BuscarDados(nome, DateTime.Now.ToString("dd/MM/yyyy"));

                        if (conexao.entrada==null)
                        {
                            conexao.InserirDadosRegistro(nome, DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm:ss"));
                        }
                        else
                        {
                            if (conexao.saida_Pausa1.ToString().Equals(""))
                            {
                                
                                TimeSpan t1 = new TimeSpan(int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(0, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(3, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(6, 2)));
                                TimeSpan t2 = new TimeSpan(int.Parse(conexao.entrada.ToString().Substring(0, 2)), int.Parse(conexao.entrada.ToString().Substring(3, 2)), int.Parse(conexao.entrada.ToString().Substring(6, 2)));
                                TimeSpan t3 = t1 - t2;
                                if (t3.TotalMinutes>10)
                                {
                                    conexao.AlterarDados(nome, DateTime.Now.ToString("dd/MM/yyyy"), "saida_Pausa1");
                                }
                                
                            }
                            else
                            {
                                if (conexao.entrada_Pausa1.ToString().Equals(""))
                                {
                                    TimeSpan t1 = new TimeSpan(int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(0, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(3, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(6, 2)));
                                    TimeSpan t2 = new TimeSpan(int.Parse(conexao.saida_Pausa1.ToString().Substring(0, 2)), int.Parse(conexao.saida_Pausa1.ToString().Substring(3, 2)), int.Parse(conexao.saida_Pausa1.ToString().Substring(6, 2)));
                                    TimeSpan t3 = t1 - t2;
                                    if (t3.TotalMinutes > 10)
                                    {
                                        conexao.AlterarDados(nome, DateTime.Now.ToString("dd/MM/yyyy"), "entrada_Pausa1");
                                    }
                                   
                                }
                                else
                                {
                                    if (conexao.saida_Pausa2.ToString().Equals(""))
                                    {
                                        TimeSpan t1 = new TimeSpan(int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(0, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(3, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(6, 2)));
                                        TimeSpan t2 = new TimeSpan(int.Parse(conexao.entrada_Pausa1.ToString().Substring(0, 2)), int.Parse(conexao.entrada_Pausa1.ToString().Substring(3, 2)), int.Parse(conexao.entrada_Pausa1.ToString().Substring(6, 2)));
                                        TimeSpan t3 = t1 - t2;
                                        if (t3.TotalMinutes > 10)
                                        {
                                            conexao.AlterarDados(nome, DateTime.Now.ToString("dd/MM/yyyy"), "saida_Pausa2");
                                        }
                                    }
                                    else
                                    {
                                        if (conexao.entrada_Pausa2.ToString().Equals(""))
                                        {
                                            TimeSpan t1 = new TimeSpan(int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(0, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(3, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(6, 2)));
                                            TimeSpan t2 = new TimeSpan(int.Parse(conexao.saida_Pausa2.ToString().Substring(0, 2)), int.Parse(conexao.saida_Pausa2.ToString().Substring(3, 2)), int.Parse(conexao.saida_Pausa2.ToString().Substring(6, 2)));
                                            TimeSpan t3 = t1 - t2;
                                            if (t3.TotalMinutes > 10)
                                            {
                                                conexao.AlterarDados(nome, DateTime.Now.ToString("dd/MM/yyyy"), "entrada_Pausa2");
                                            }
                                            
                                        }
                                        else
                                        {
                                            if (conexao.saida.ToString().Equals(""))
                                            {
                                                TimeSpan t1 = new TimeSpan(int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(0, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(3, 2)), int.Parse(DateTime.Now.ToString("HH:mm:ss").Substring(6, 2)));
                                                TimeSpan t2 = new TimeSpan(int.Parse(conexao.entrada_Pausa2.ToString().Substring(0, 2)), int.Parse(conexao.entrada_Pausa2.ToString().Substring(3, 2)), int.Parse(conexao.entrada_Pausa2.ToString().Substring(6, 2)));
                                                TimeSpan t3 = t1 - t2;
                                                if (t3.TotalMinutes > 10)
                                                {
                                                    conexao.AlterarDados(nome, DateTime.Now.ToString("dd/MM/yyyy"), "saida");
                                                }
                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        conexao.ObjVazio();
                    }
                  
                }


            }
        }
        DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)

                return features;
            else
                return null;
        }
        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
          
        }
        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            img = null;
        }
        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {

        }
        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {

        }
        public void OnSampleQuality(object Capture, string ReaderSerialNumber, CaptureFeedback CaptureFeedback)
        {

        }
        #endregion

        #region Interface
      
        private void BtnCadastro_Click(object sender, EventArgs e)
        {
            FrmCadastro f = new FrmCadastro();
            cp.StopCapture();
            f.ShowDialog();
        }
        private void TimerHora_Tick(object sender, EventArgs e)
        {
            lbldata.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblhora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        void CadastroEncontrado()
        {
           
            lblNome.Text = nome;
            //lblId.Text = id.ToString();
            lblStatus.Text = "Cadastrado";
            cadastrado = false;
           
            status = true;

        }
        void CadastroNaoEncontrado()
        {
           
            lblStatus.Text = "Não Cadastrado";
           
            status = true;
        }
        void LimparTela()
        {
            status = false;
            Bitmap imgnull = null;
            pictureBox1.Image = imgnull;
            lblNome.Text = "..........";
            //lblId.Text = "..........";
            lblStatus.Text = "Status";
        }

        #endregion

        private void TimerStatus_Tick(object sender, EventArgs e)
        {
            if (status)
            {
                LimparTela();
            }
        }

        private void BtnRegistro_Click(object sender, EventArgs e)
        {
            FrmResgistro f = new FrmResgistro();
            f.ShowDialog();
        }
    }
}
