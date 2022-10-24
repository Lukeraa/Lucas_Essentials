using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Lucas_Essentials
{
    class Banco
    {
        private static SQLiteConnection conexao;

        //
        // Método para conectar ao banco de dados.
        //
        public static SQLiteConnection ConexaoBanco()
        {
            conexao = new SQLiteConnection("Data Source=E:\\AreaDeTrabalho\\C#\\WindowsForms\\Lucas_Essentials-Bk4\\banco\\banco_academia.db");
            //conexao = new SQLiteConnection("Data Source=" + Globais.caminho_banco);
            conexao.Open();
            return conexao;
        }

        //
        // Método para obter todos os usuários.
        //
        public static DataTable ObterTodosUsuarios()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = "SELECT * FROM tb_usuarios";
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);
                da.Fill(dt);
                vcon.Close();
                return dt;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        //
        // Método genérico para fazer consultas no banco de dados.
        //
        public static DataTable dql(string sql) // Data Query Language (select)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = sql;
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);
                da.Fill(dt);
                vcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // Método genérico para fazer modificações no banco de dados.
        //
        public static void dml(string q, string msgOK=null, string msgErro=null) // Data Manipulation Language (Insert, Delete, Update)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = q;
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);
                cmd.ExecuteNonQuery();
                vcon.Close();
                if (msgOK != null)
                {
                    MessageBox.Show(msgOK);
                }
            }
            catch (Exception ex)
            {
                if (msgErro != null)
                {
                    MessageBox.Show(msgErro + "\n" + ex.Message);
                }
                throw ex;
            }
        }

        //
        // Método para verificar se o usuário já está cadastrado.
        //
        public static bool ExisteUser(Usuario u)
        {
            bool res;
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();

            var vcon = ConexaoBanco();
            var cmd = vcon.CreateCommand();
            //cmd.CommandText = "SELECT T_USER FROM tb_usuarios WHERE T_USER = '"+u.T_USER+"'";
            cmd.CommandText = "SELECT T_USER FROM tb_usuarios WHERE T_USER = @user";
            cmd.Parameters.AddWithValue("@user", u.T_USER);
            da = new SQLiteDataAdapter(cmd.CommandText, vcon);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                res = true;
            }
            else
            {
                res = false;
            }
            vcon.Close();
            return res;
        }

        //
        // Método para cadastrar novo usuário.
        //
        public static void NovoUser(Usuario u)
        {
            if (ExisteUser(u))
            {
                MessageBox.Show("Esse nome de usuário já está cadastrado.", "Erro no cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = "INSERT INTO tb_usuarios (T_NOME, T_USER, T_SENHA, T_STATUS, N_NVL) VALUES (@nome, @user, @senha, @status, @nvl)";
                cmd.Parameters.AddWithValue("@nome", u.T_NOME);
                cmd.Parameters.AddWithValue("@user", u.T_USER);
                cmd.Parameters.AddWithValue("@senha", u.T_SENHA);
                cmd.Parameters.AddWithValue("@status", u.T_STATUS);
                cmd.Parameters.AddWithValue("@nvl", u.N_NVL);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Novo usuário cadastrado!", "Sucesso", MessageBoxButtons.OK);
                vcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //
        // Método para obter nome e id do banco.
        //
        public static DataTable ObterUsuariosIdNome()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = "SELECT N_ID as 'Id', T_NOME as 'Nome' FROM tb_usuarios";
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);
                da.Fill(dt);
                vcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // Método para obter nome e id do banco.
        //
        public static DataTable ObterDadosUsuarios(string id)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = "SELECT * FROM tb_usuarios WHERE N_ID = " + id;
                //cmd.Parameters.AddWithValue("@id", id);
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);
                da.Fill(dt);
                vcon.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // Método para atualizar usuários no banco de dados.
        //
        public static void AtualizarUusuario(Usuario u)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = "UPDATE tb_usuarios SET T_NOME=@nome, T_USER=@user, T_SENHA=@senha, T_STATUS=@status N_NVL=@nvl WHERE N_ID=@id";
                cmd.Parameters.AddWithValue("@nome", u.T_NOME);
                cmd.Parameters.AddWithValue("@user", u.T_USER);
                cmd.Parameters.AddWithValue("@senha", u.T_SENHA);
                cmd.Parameters.AddWithValue("@status", u.T_STATUS);
                cmd.Parameters.AddWithValue("@nvl", u.N_NVL);
                cmd.Parameters.AddWithValue("@id", u.N_ID);
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);
                cmd.ExecuteNonQuery();
                vcon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        // Método para excluir um usuário.
        //
        public static void Excluir(string id)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                var vcon = ConexaoBanco();
                var cmd = vcon.CreateCommand();
                cmd.CommandText = "DELETE FROM tb_usuarios WHERE N_ID=@id";
                cmd.Parameters.AddWithValue("@id", id);
                da = new SQLiteDataAdapter(cmd.CommandText, vcon);
                cmd.ExecuteNonQuery();
                vcon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
