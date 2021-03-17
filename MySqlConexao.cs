using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace LeitorDigital3
{
    class MySqlConexao
    {
        public object nome;
       
        public object data;
        public object entrada;
        public object saida_Pausa1;
        public object entrada_Pausa1;
        public object saida_Pausa2;
        public object entrada_Pausa2;
        public object saida;
        public object total_Hora_Dia;


        MySqlConnection conexao = ConexaoDB.getInstancia().getConexao();
        public void InserirDados( byte[] FPTemp, string Nome)
        {

            try
            {
                conexao.Open();

                MySqlCommand comando = conexao.CreateCommand();
                comando.CommandText = "insert into tb_cadastro (FPTemp, Nome) values ( @FPTemp, @Nome);";
              
                comando.Parameters.AddWithValue("FPTemp", FPTemp);
                comando.Parameters.AddWithValue("Nome", Nome);

                if (comando.ExecuteNonQuery() <= 1)
                {
                    MessageBox.Show("Sucesso ao Inserir", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Erro ao Inserir");
                }
            }
            catch (Exception a)
            {

                MessageBox.Show("Erro: " + a.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();

            }


        }
        public void BuscarDadosLista(DataTable dtLista)
        {
            try
            {
                string strgComando = "SELECT * FROM tb_cadastro;";
                if (conexao.State == ConnectionState.Closed)
                {
                    conexao.Open();
                }
                MySqlCommand comando = new MySqlCommand(strgComando, conexao);


                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(comando);
                sqlDataAdapter.Fill(dtLista);
            }
            catch (Exception a)
            {
                MessageBox.Show("Erro: " + a.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }
        }
        public void InserirDadosRegistro(string nome, string data, string entrada)
        {

            try
            {
                conexao.Open();

                MySqlCommand comando = conexao.CreateCommand();
                comando.CommandText = "insert into tb_registro (nome, data, entrada) values (@nome, @data, @entrada);";
                comando.Parameters.AddWithValue("nome", nome);
               
                comando.Parameters.AddWithValue("data", data);
                comando.Parameters.AddWithValue("entrada", entrada);

                if (comando.ExecuteNonQuery() <= 1)
                {
                    //MessageBox.Show("Sucesso ao Inserir", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Erro ao Inserir");
                }
            }
            catch (Exception a)
            {

                MessageBox.Show("Erro: " + a.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();

            }


        }
        public void BuscarDados(string nomef ,string dataf)
        {
            try
            {
                if (conexao.State == ConnectionState.Closed)
                {
                    conexao.Open();
                }

                MySqlCommand comando = conexao.CreateCommand();
                comando.CommandText = "select * from tb_registro where nome = @nome && data = @data;";
                comando.Parameters.AddWithValue("nome", nomef);
                comando.Parameters.AddWithValue("data", dataf);

                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["nome"] != null)
                    {
                        nome = reader["nome"].ToString();
                        data = reader["data"].ToString();
                        entrada = reader["entrada"].ToString();
                        saida_Pausa1 = reader["saida_Pausa1"].ToString();
                        entrada_Pausa1 = reader["entrada_Pausa1"].ToString();
                        saida_Pausa2 = reader["saida_Pausa2"].ToString();
                        entrada_Pausa2 = reader["entrada_Pausa2"].ToString();
                        saida = reader["saida"].ToString();
                        total_Hora_Dia = reader["total_Hora_Dia"].ToString();
                    }
                    else
                    {
                        //MessageBox.Show("id Invalido");
                    }
                }
            }
            catch (Exception a)
            {
                MessageBox.Show("Erro: " + a.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                conexao.Close();
            }

        }
        public void ObjVazio()
        {
            nome = null;
            
            data = null;
            entrada = null;
            saida_Pausa1 = null;
            entrada_Pausa1 = null;
            saida_Pausa2 = null;
            entrada_Pausa2 = null;
            saida = null;
            total_Hora_Dia = null;
        }
        public void AlterarDados(string nomef, string dataf, string coluna)
        {

            try
            {
                conexao.Open();
                MySqlCommand comando = conexao.CreateCommand();
                comando.CommandText = "update tb_registro set " + coluna+" = @hora  where  data = @data && nome = @nome;";
                comando.Parameters.AddWithValue("nome", nomef);
                comando.Parameters.AddWithValue("data", dataf);
                comando.Parameters.AddWithValue("hora", DateTime.Now.ToString("HH:mm:ss"));

                if (comando.ExecuteNonQuery() <= 1)
                {
                    // MessageBox.Show("Sucesso ao Atualizar", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //  MessageBox.Show("Erro ao Atualizar", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception a)
            {
                MessageBox.Show("Erro: " + a.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }


        }
        public void CarregarDataGrid(DataGridView dataGridView)
        {
            try
            {
                string strgComando = "SELECT * FROM dbregistroponto.tb_registro;";

                if (conexao.State == ConnectionState.Closed)
                {
                    conexao.Open();
                }
                MySqlCommand comando = new MySqlCommand(strgComando, conexao);


                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(comando);

                DataTable dtLista = new DataTable();

                sqlDataAdapter.Fill(dtLista);
                dataGridView.DataSource = dtLista;

                dataGridView.Columns[0].Width = 50;
                dataGridView.Columns[1].Width = 200;
                dataGridView.Columns[2].Width = 80;
                dataGridView.Columns[3].Width = 80;
                dataGridView.Columns[4].Width = 80;
                dataGridView.Columns[5].Width = 80;
                dataGridView.Columns[6].Width = 80;
                dataGridView.Columns[7].Width = 80;
                dataGridView.Columns[8].Width = 80;
                dataGridView.Columns[9].Width = 80;
               
            }
            catch (Exception a)
            {
                MessageBox.Show("Erro: " + a.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }

        }
    }
}
