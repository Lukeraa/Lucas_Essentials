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
    public partial class F_Horarios : Form
    {
        public F_Horarios()
        {
            InitializeComponent();
        }

        private void dgv_horarios_Load(object sender, EventArgs e)
        {
            string vquery = @"SELECT H_ID as 'ID', H_DSC as 'Horário' FROM tb_horarios ORDER BY H_DSC";
            dgv_horarios.DataSource = Banco.dql(vquery);
            dgv_horarios.Columns[0].Width = 60;
            dgv_horarios.Columns[1].Width = 170;
        }

        private void dgv_horarios_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                DataTable dt = new DataTable();
                string id = dgv.SelectedRows[0].Cells[0].Value.ToString();
                string vquery = "SELECT * FROM tb_horarios WHERE H_ID="+id;
                dt = Banco.dql(vquery);
                tb_id.Text = dt.Rows[0].Field<Int64>("H_ID").ToString();
                mtb_dsc.Text = dt.Rows[0].Field<string>("H_DSC");
            }
        }

        private void btn_novo_Click(object sender, EventArgs e)
        {
            tb_id.Clear();
            mtb_dsc.Clear();
            mtb_dsc.Focus();
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            string vquery;
            if (tb_id.Text == "")
            {
                vquery = "INSERT INTO tb_horarios (H_DSC) VALUES ('" + mtb_dsc.Text + "')";
            }
            else
            {
                vquery = "UPDATE tb_horarios SET H_DSC ='" + mtb_dsc.Text + "' WHERE H_ID =" + tb_id.Text;
            }
            Banco.dml(vquery);
            vquery = "SELECT H_ID as 'ID', H_DSC as 'Horário' FROM tb_horarios ORDER BY H_DSC";
            dgv_horarios.DataSource = Banco.dql(vquery);
        }

        private void btn_excluir_Click(object sender, EventArgs e)
        {
            var exc = MessageBox.Show("Tem certeza que deseja excluir o horário [" + dgv_horarios.SelectedRows[0].Cells[1].Value + "] do banco de dados?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (exc == DialogResult.Yes)
            {
                string vquery = "DELETE FROM tb_horarios WHERE H_ID ="+tb_id.Text;
                Banco.dml(vquery);
                dgv_horarios.Rows.Remove(dgv_horarios.CurrentRow);
            }
        }

        private void btn_fechar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
