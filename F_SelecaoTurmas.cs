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
    public partial class F_SelecaoTurmas : Form
    {
        F_NovoAluno fnovo;
        public F_SelecaoTurmas(F_NovoAluno f )
        {
            InitializeComponent();
            fnovo = f;
        }

        private void F_SelecaoTurmas_Load(object sender, EventArgs e)
        {
            string vquery = String.Format(@"SELECT
                                                tbt.T_ID as 'ID', 
                                                tbt.T_DSC as 'Turma',
                                                tbh.H_DSC as 'Horário',
                                                tbp.P_NOME as 'Professor',
                                                (
                                                    SELECT
                                                        count(A_ID)
                                                    FROM
                                                        tb_alunos as 'tba'
                                                    WHERE
                                                        tba.A_IDTURMA = tbt.T_ID and A_STATUS = 'A'
                                                ) as 'Qtd. Alunos',
                                                tbt.T_MAXALUNOS as 'Máx. Alunos'
                                            FROM 
                                                tb_turmas as tbt 
                                            INNER JOIN
                                                tb_professores as tbp on tbp.P_ID = tbt.T_IDPROF
                                            INNER JOIN
                                                tb_horarios as tbh on tbh.H_ID = tbt.T_IDHORARIO
                                            ORDER BY 
                                                Horário
                                        ");
            dgv_SelecT.DataSource = Banco.dql(vquery);
            dgv_SelecT.Columns[0].Width = 50;
            dgv_SelecT.Columns[1].Width = 155;
        }

        private void dgv_SelecT_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            int maxAlunos = 0;
            int qtdAlunos = 0;

            maxAlunos = Int32.Parse(dgv.SelectedRows[0].Cells[5].Value.ToString());
            qtdAlunos = Int32.Parse(dgv.SelectedRows[0].Cells[4].Value.ToString());

            if (qtdAlunos >= maxAlunos)
            {
                MessageBox.Show("Não foi possível selecionar a turma "+dgv.Rows[dgv.SelectedRows[0].Index].Cells[1].Value.ToString()+"!", "Turma cheia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                fnovo.tb_turma.Text = dgv.Rows[dgv.SelectedRows[0].Index].Cells[1].Value.ToString();
                fnovo.tb_turma.Tag = dgv.Rows[dgv.SelectedRows[0].Index].Cells[0].Value.ToString();
                Close();
            }
        }
    }
}
