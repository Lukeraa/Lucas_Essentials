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
    public partial class F_GestaoProfessores : Form
    {
        public F_GestaoProfessores()
        {
            InitializeComponent();
        }

        private void F_GestaoProfessores_Load(object sender, EventArgs e)
        {
            string vquery = "SELECT P_ID as 'ID', P_NOME as 'Professor(a)', P_TELEFONE as 'Telefone' FROM tb_professores ORDER BY P_NOME";
            dgv_professores.DataSource = Banco.dql(vquery);
            dgv_professores.Columns[0].Width = 80;
            dgv_professores.Columns[1].Width = 210;
            dgv_professores.Columns[2].Width = 131;
        }

        private void dgv_professores_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                DataTable dt = new DataTable();
                string id = dgv_professores.SelectedRows[0].Cells[0].Value.ToString();
                string vquery = "SELECT * FROM tb_professores WHERE P_ID=@id";
                string vquery2 = vquery.Replace("@id", id);
                dt = Banco.dql(vquery2);
                tb_id.Text = dt.Rows[0].Field<Int64>("P_ID").ToString();
                tb_professor.Text = dt.Rows[0].Field<string>("P_NOME");
                mtb_telefone.Text = dt.Rows[0].Field<string>("P_TELEFONE");
            }
        }

        private void btn_novo_Click(object sender, EventArgs e)
        {
            tb_id.Clear();
            tb_professor.Clear();
            mtb_telefone.Clear();
            tb_professor.Focus();
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            string vquery;
            if (tb_id.Text == "")
            {
                vquery = "INSERT INTO tb_professores (P_NOME, P_TELEFONE) VALUES ('" + tb_professor.Text+"', '" + mtb_telefone.Text + "')";
            }
            else
            {
                vquery = "UPDATE tb_professores SET P_NOME = '"+tb_professor.Text+"', P_TELEFONE = '"+mtb_telefone.Text+"' WHERE P_ID = '"+tb_id.Text+"'";
            }
            Banco.dml(vquery);
            vquery = "SELECT P_ID as 'ID', P_NOME as 'Professor(a)', P_TELEFONE as 'Telefone' FROM tb_professores ORDER BY P_NOME";
            dgv_professores.DataSource = Banco.dql(vquery);
        }

        private void btn_excluir_Click(object sender, EventArgs e)
        {
            var exc = MessageBox.Show("Tem certeza que deseja excluir ["+dgv_professores.SelectedRows[0].Cells[1].Value+"] do banco de dados?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (exc == DialogResult.Yes)
            {
                string vquery = "DELETE FROM tb_professores WHERE P_ID="+tb_id.Text;
                Banco.dml(vquery);
                dgv_professores.Rows.Remove(dgv_professores.CurrentRow);
            }
        }

        private void btn_fechar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
