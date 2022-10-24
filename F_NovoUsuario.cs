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
    public partial class F_NovoUsuario : Form
    {
        public F_NovoUsuario()
        {
            InitializeComponent();
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            if (tb_nome.Text != "" & tb_user.Text != "" & tb_senha.Text != "" & cb_status.Text != "")
            {
                Usuario usuario = new Usuario();
                usuario.T_NOME = tb_nome.Text;
                usuario.T_USER = tb_user.Text;
                usuario.T_SENHA = tb_senha.Text;
                usuario.T_STATUS = cb_status.Text;
                usuario.N_NVL = Convert.ToInt32(Math.Round(nup_nvl.Value, 0));

                Banco.NovoUser(usuario);
                this.Close();
            }
            else
            {
                MessageBox.Show("Preencha todos os campos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tb_nome.Focus();
            }
        }

        private void btn_fechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            tb_user.Clear();
            tb_nome.Clear();
            tb_senha.Clear();
            cb_status.Text = "";
            nup_nvl.Value = 1;

            tb_nome.Focus();    
        }
    }
}
