using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lucas_Essentials
{
    public partial class F_Login : Form
    {
        DataTable dt = new DataTable();

        //
        // Fazer formulário filho conseguir enviar dados ao pai.
        //
        Form1 form1;

        public F_Login(Form1 f)
        {
            InitializeComponent();
            form1 = f;
        }

        //
        // Método para verificar se há campos vazios, fazer o login, limitar o menu e mudar informações.
        //
        private void btn_entrar_Click(object sender, EventArgs e)
        {
            string user = tb_user.Text;
            string senha = tb_senha.Text;

            //
            // Ver se há campos vazios.
            //
            if (user == "" | senha == "")
            {
                MessageBox.Show("'Usuário' e 'Senha' devem estar preenchidos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_user.Focus();
                return;
            }

            //
            // Fazer o login.
            //
            string sql = "SELECT * FROM tb_usuarios WHERE T_USER ='@user' AND T_SENHA ='@senha'";
            string sql2 = sql.Replace("@user", user);
            string sql3 = sql2.Replace("@senha", senha);
            dt = Banco.dql(sql3);
            if (dt.Rows.Count == 1)
            {
                form1.lb_acesso.Text = dt.Rows[0].ItemArray[5].ToString();
                form1.lb_nomeuser.Text = dt.Rows[0].Field<string>("T_NOME");
                string nomeuser = dt.Rows[0].Field<string>("T_NOME");
                form1.pb_ledlogado.Image = Properties.Resources.verde;
                Globais.nvl = int.Parse(dt.Rows[0].Field<Int64>("N_NVL").ToString());
                Globais.logado = true;
                //
                // Limitar menu de acordo com nível de acesso.
                //
                if (Globais.nvl == 3)
                {
                    form1.alunosToolStripMenuItem.Visible = true;
                    form1.usuáriosToolStripMenuItem.Visible = true;
                    form1.manutençãoToolStripMenuItem.Visible = true;
                    form1.lOGARToolStripMenuItem.Visible = false;
                    MessageBox.Show("Bem-vindo(a) "+nomeuser, "Chefe");
                }
                else if (Globais.nvl == 2)
                {
                    form1.alunosToolStripMenuItem.Visible = true;
                    form1.usuáriosToolStripMenuItem.Visible = true;
                    form1.manutençãoToolStripMenuItem.Visible = false;
                    form1.lOGARToolStripMenuItem.Visible = false;
                    MessageBox.Show("Bem-vindo(a) "+nomeuser, "Gerente");
                }
                else if (Globais.nvl == 1)
                {
                    form1.alunosToolStripMenuItem.Visible = true;
                    form1.usuáriosToolStripMenuItem.Visible = false;
                    form1.manutençãoToolStripMenuItem.Visible = false;
                    form1.lOGARToolStripMenuItem.Visible = false;
                    MessageBox.Show("Bem-vindo(a) "+nomeuser, "Aluno(a)");
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuário não encontrado", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb_user.Focus();
            }
        }

        //
        // Método para fechar programa
        //
        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
