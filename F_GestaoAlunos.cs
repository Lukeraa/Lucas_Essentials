using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Lucas_Essentials
{
    public partial class F_GestaoAlunos : Form
    {
        string vquery = "";
        string turmaAtual = "";
        string idSelecionado = "";
        string turma = "";
        int linha = 0;
        public F_GestaoAlunos()
        {
            InitializeComponent();
        }

        private void F_GestaoAlunos_Load(object sender, EventArgs e)
        {
            vquery = String.Format("SELECT A_ID as 'ID', A_NOME as 'Aluno' FROM tb_alunos", "");
            dgv_alunos.DataSource = Banco.dql(vquery);
            dgv_alunos.Columns[0].Width = 40;
            dgv_alunos.Columns[1].Width = 247;

            tb_nome.Text = dgv_alunos.Rows[dgv_alunos.SelectedRows[0].Index].Cells[1].Value.ToString();

            vquery = @"
                        SELECT 
                            T_ID,
                            ('Vagas: ' || (
                                                (T_MAXALUNOS)-(
                                                            SELECT
                                                                count(tba.A_ID)
                                                            FROM
                                                                tb_alunos as tba
                                                            WHERE
                                                                tba.A_STATUS = 'A' and tba.A_IDTURMA = T_ID
                                                                )
                                            ) || ' / Turma: ' || T_DSC
                            )as 'Turma'
                        FROM
                            tb_turmas
                        ORDER BY
                            T_ID
           ";
            cb_turmas.Items.Clear();
            cb_turmas.DataSource = Banco.dql(vquery);
            cb_turmas.DisplayMember = "Turma";
            cb_turmas.ValueMember = "T_ID";

            Dictionary<string, string> status = new Dictionary<string, string>();
            status.Add("A", "Ativo");
            status.Add("B", "Bloqueado");
            status.Add("C", "Cancelado");
            cb_status.DataSource = new BindingSource(status, null);
            cb_status.DisplayMember = "Value";
            cb_status.ValueMember = "Key";

            turma = cb_turmas.Text;
            turmaAtual = cb_turmas.Text;
            idSelecionado = dgv_alunos.Rows[0].Cells[0].Value.ToString();
        }

        private void dgv_alunos_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.SelectedRows.Count > 0)
            {
                idSelecionado = dgv.Rows[dgv.SelectedRows[0].Index].Cells[0].Value.ToString();
                vquery = string.Format("SELECT A_ID, A_NOME, A_TELEFONE, A_STATUS, A_IDTURMA, A_FOTO FROM tb_alunos WHERE A_ID = {0}", idSelecionado);
                DataTable dt = Banco.dql(vquery);
                tb_nome.Text = dt.Rows[0].Field<string>("A_NOME");
                mtb_telefone.Text = dt.Rows[0].Field<string>("A_TELEFONE");
                cb_status.SelectedValue = dt.Rows[0].Field<string>("A_STATUS");
                cb_turmas.SelectedValue = dt.Rows[0].Field<Int64>("A_IDTURMA");
                pb_fotoAluno.ImageLocation = dt.Rows[0].Field<string>("A_FOTO");
                turmaAtual = cb_turmas.Text;
            }
        }

        private void btn_Salvar_Click(object sender, EventArgs e)
        {
            turma = cb_turmas.Text;
            if (turmaAtual != turma)
            {
                string[] v = turma.Split(' ');
                int vagas = int.Parse(v[1]);
                string[] t = turma.Split(' ');
                string tzin = t[4];
                if (vagas < 1)
                {
                    MessageBox.Show("Não foi possível selecionar a turma " + tzin + "!", "Turma cheia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cb_turmas.Focus();
                    return;
                }
                vquery = String.Format("UPDATE tb_alunos SET A_NOME = '{0}', A_TELEFONE = '{1}', A_STATUS = '{2}', A_IDTURMA = '{3}' WHERE A_ID = {4}", tb_nome.Text, mtb_telefone.Text, cb_status.SelectedValue, cb_turmas.SelectedValue, idSelecionado);
                Banco.dml(vquery);
                dgv_alunos[1, dgv_alunos.SelectedRows[0].Index].Value = tb_nome.Text;
                vquery = @"
                        SELECT 
                            T_ID,
                            ('Vagas: ' || (
                                                (T_MAXALUNOS)-(
                                                            SELECT
                                                                count(tba.A_ID)
                                                            FROM
                                                                tb_alunos as tba
                                                            WHERE
                                                                tba.A_STATUS = 'A' and tba.A_IDTURMA = T_ID
                                                                )
                                            ) || ' / Turma: ' || T_DSC
                            )as 'Turma'
                        FROM
                            tb_turmas
                        ORDER BY
                            T_ID
                ";
                cb_turmas.DataSource = Banco.dql(vquery);
                cb_turmas.DisplayMember = "Turma";
                cb_turmas.ValueMember = "T_ID";
                MessageBox.Show("Alterações salvas com sucesso!", "Salvar Alterações", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                vquery = String.Format("UPDATE tb_alunos SET A_NOME = '{0}', A_TELEFONE = '{1}', A_STATUS = '{2}', A_IDTURMA = '{3}' WHERE A_ID = {4}", tb_nome.Text, mtb_telefone.Text, cb_status.SelectedValue, cb_turmas.SelectedValue, idSelecionado);
                Banco.dml(vquery);
                dgv_alunos[1, dgv_alunos.SelectedRows[0].Index].Value = tb_nome.Text;
                MessageBox.Show("Alterações salvas com sucesso!", "Salvar Alterações", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void btn_Excluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Excluir "+tb_nome.Text+ "do banco de dados?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if(File.Exists(pb_fotoAluno.ImageLocation))
                {
                    File.Delete(pb_fotoAluno.ImageLocation);
                }
                vquery = String.Format("DELETE FROM tb_alunos WHERE A_ID = {0}", idSelecionado);
                Banco.dml(vquery);
                dgv_alunos.Rows.Remove(dgv_alunos.CurrentRow);
            }
        }

        private void btn_Fechar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
