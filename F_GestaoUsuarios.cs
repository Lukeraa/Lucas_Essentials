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
    public partial class F_GestaoUsuarios : Form
    {
        public F_GestaoUsuarios()
        {
            InitializeComponent();
        }

        private void btn_fechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void F_GestaoUsuarios_Load(object sender, EventArgs e)
        {
            dgv_usuarios.DataSource = Banco.ObterUsuariosIdNome();
            dgv_usuarios.Columns[0].Width = 88;
            dgv_usuarios.Columns[1].Width = 280;
        }

        private void dgv_usuarios_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            int contlinhas = dgv.SelectedRows.Count;
            if (contlinhas > 0)
            {
                DataTable dt = new DataTable();
                string vid = dgv.SelectedRows[0].Cells[0].Value.ToString();
                dt = Banco.ObterDadosUsuarios(vid);
                tb_id.Text = dt.Rows[0].Field<Int64>("N_ID").ToString();
                tb_nome.Text = dt.Rows[0].Field<string>("T_NOME").ToString();
                tb_user.Text = dt.Rows[0].Field<string>("T_USER").ToString();
                tb_senha.Text = dt.Rows[0].Field<string>("T_Senha").ToString();
                cb_status.Text = dt.Rows[0].Field<string>("T_STATUS").ToString();
                nup_nvl.Value = dt.Rows[0].Field<Int64>("N_NVL");
            }
        }

        //
        // Acionar formulário de cadastro de usuário.
        //
        private void btn_novo_Click(object sender, EventArgs e)
        {
            F_NovoUsuario f_NovoUsuario = new F_NovoUsuario();
            f_NovoUsuario.ShowDialog();
            dgv_usuarios.DataSource = Banco.ObterUsuariosIdNome();

        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            int linha = dgv_usuarios.SelectedRows[0].Index;
            Usuario u = new Usuario();
            u.N_ID = Convert.ToInt32(tb_id.Text);
            u.T_NOME = tb_nome.Text;
            u.T_USER = tb_user.Text;
            u.T_SENHA = tb_senha.Text;
            u.T_STATUS = cb_status.Text;
            u.N_NVL = Convert.ToInt32(Math.Round(nup_nvl.Value));
            Banco.AtualizarUusuario(u);
            //dgv_usuarios.DataSource = Banco.ObterUsuariosIdNome();
            //dgv_usuarios.CurrentCell = dgv_usuarios[0, linha];
            dgv_usuarios[1, linha].Value = tb_nome.Text;
        }

        private void btn_excluir_Click(object sender, EventArgs e)
        {
            var exc = MessageBox.Show("Tem certeza que deseja excluir ["+tb_nome.Text+"] do banco de dados?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (exc == DialogResult.Yes)
            {
                Banco.Excluir(tb_id.Text);
                dgv_usuarios.Rows.Remove(dgv_usuarios.CurrentRow);
                dgv_usuarios.Rows[0].Selected = true;
            }
        }
    }
}
