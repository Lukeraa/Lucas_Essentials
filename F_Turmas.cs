using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Lucas_Essentials
{
    public partial class F_Turmas : Form
    {
        string idSelecionado;
        string vquery = "";
        string vqueryler = "";
        public F_Turmas()
        {
            InitializeComponent();
        }

        private void F_Turmas_Load(object sender, EventArgs e)
        {
            vqueryler = @"
                            SELECT 
                                tbt.T_ID as 'ID', 
                                tbt.T_DSC as 'Matéria',
                                tbh.H_DSC as 'Horário'
                            FROM
                                tb_turmas as 'tbt'
                            inner join
                                tb_horarios as 'tbh' on tbh.H_ID = tbt.T_IDHORARIO
                            ORDER BY
                                Horário
                            ";
            dgv_turmas.DataSource = Banco.dql(vqueryler);
            dgv_turmas.Columns[0].Width = 40;
            dgv_turmas.Columns[1].Width = 120;
            dgv_turmas.Columns[2].Width = 85;

            //
            // Popular ComboBox professores
            //
            vquery = @"
                SELECT
                    P_ID,
                    P_NOME
                FROM
                    tb_professores
                ORDER BY
                    P_NOME
            ";
            cb_prof.Items.Clear();
            cb_prof.DataSource = Banco.dql(vquery);
            cb_prof.DisplayMember = "P_NOME";
            cb_prof.ValueMember = "P_ID";

            //
            // Popular ComboBox Status
            //
            Dictionary<string, string> st = new Dictionary<string, string>();
            st.Add("A","Ativa");
            st.Add("P","Paralisada");
            st.Add("C","Cancelada");
            cb_status.Items.Clear();
            cb_status.DataSource = new BindingSource(st, null);
            cb_status.DisplayMember = "Value";
            cb_status.ValueMember = "Key";

            //
            // Popular ComboBox Horários
            //
            vquery = @"
                SELECT
                    H_ID,
                    H_DSC
                FROM
                    tb_horarios
                ORDER BY
                    H_DSC
            ";
            cb_horario.Items.Clear();
            cb_horario.DataSource = Banco.dql(vquery);
            cb_horario.DisplayMember = "H_DSC";
            cb_horario.ValueMember = "H_ID";
        }

        //
        // Configurando o SelectionChanged do dgv
        //
        private void dgv_turmas_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv_turmas.SelectedRows.Count > 0)
            {
                tb_edisalva.Text = "1";
                idSelecionado = dgv_turmas.Rows[dgv_turmas.SelectedRows[0].Index].Cells[0].Value.ToString();
                string vquery = @"
                SELECT
                    T_DSC,
                    T_IDPROF,
                    T_IDHORARIO,
                    T_MAXALUNOS,
                    T_STATUS
                    
                FROM
                    tb_turmas
                WHERE
                    T_ID=" + idSelecionado;
                DataTable dt = Banco.dql(vquery);
                tb_dscturma.Text = dt.Rows[0].Field<string>("T_DSC");
                cb_prof.SelectedValue = dt.Rows[0].Field<Int64>("T_IDPROF").ToString();
                nup_maxalunos.Value = dt.Rows[0].Field<Int64>("T_MAXALUNOS");
                cb_status.SelectedValue = dt.Rows[0].Field<string>("T_STATUS");
                cb_horario.SelectedValue = dt.Rows[0].Field<Int64>("T_IDHORARIO");

                //
                // Cálculo de vagas.
                //
                calcVagas();
            }
        }

        //
        // Cálculo de vagas.
        //
        private void calcVagas()
        {
            string vquery = String.Format("SELECT count(A_ID) as 'contVagas' FROM tb_alunos WHERE A_STATUS='A' and A_IDTURMA={0}", idSelecionado);
            DataTable dt = Banco.dql(vquery);
            int vagas = Convert.ToInt32(Math.Round(nup_maxalunos.Value, 0).ToString());
            vagas -= Convert.ToInt32(dt.Rows[0].Field<Int64>("contVagas").ToString());
            tb_vagas.Text = vagas.ToString();
        }

        //
        // Criar nova turma.
        //
        private void btn_nova_Click(object sender, EventArgs e)
        {
            tb_dscturma.Clear();
            cb_prof.SelectedIndex = -1;
            nup_maxalunos.Value = 0;
            cb_status.SelectedIndex = -1;
            cb_horario.SelectedIndex = -1;
            tb_vagas.Clear();
            tb_dscturma.Focus();
            tb_edisalva.Text = "2";
        }

        //
        // Salvar Alterações.
        //
        private void btn_salvar_Click(object sender, EventArgs e)
        {
            string vquery = "";
            if (tb_edisalva.Text == "1")
            {
                int linha = dgv_turmas.SelectedRows[0].Index;
                vquery = String.Format("UPDATE tb_turmas SET T_DSC = '{0}', T_IDPROF = {1}, T_IDHORARIO = {2}, T_MAXALUNOS = {3}, T_STATUS = '{4}' WHERE T_ID = {5}", tb_dscturma.Text, cb_prof.SelectedValue, cb_horario.SelectedValue, Convert.ToInt32(Math.Round(nup_maxalunos.Value, 0).ToString()), cb_status.SelectedValue, idSelecionado);
                dgv_turmas[1, linha].Value = tb_dscturma.Text;
                dgv_turmas[2, linha].Value = cb_horario.Text;
                Banco.dml(vquery);
                calcVagas();
                MessageBox.Show("Alterações salvas com sucesso!", "Salvar Alterações", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(tb_edisalva.Text == "2")
            {
                vquery = String.Format("INSERT INTO tb_turmas(T_DSC, T_IDPROF, T_IDHORARIO, T_MAXALUNOS, T_STATUS) VALUES('{0}', {1}, {2}, {3}, '{4}')", tb_dscturma.Text, cb_prof.SelectedValue, cb_horario.SelectedValue, Convert.ToInt32(Math.Round(nup_maxalunos.Value, 0).ToString()), cb_status.SelectedValue);
                Banco.dml(vquery);
                dgv_turmas.DataSource = Banco.dql(vqueryler);
                calcVagas();
                MessageBox.Show("Nova turma cadastrada com sucesso!", "Nova Turma", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        //
        // Excluir turma.
        //
        private void btn_excluir_Click(object sender, EventArgs e)
        {
            var exc = MessageBox.Show("Tem certeza que deseja excluir [ "+tb_dscturma.Text+" ] do banco de dados?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (exc == DialogResult.Yes)
            {
                string vquery = "DELETE FROM tb_turmas WHERE T_DSC = '"+tb_dscturma.Text+"'";
                Banco.dml(vquery);
                dgv_turmas.Rows[0].Selected = true;
                dgv_turmas.Rows.Remove(dgv_turmas.CurrentRow);
            }
        }

        //
        // Fechar formulário.
        //
        private void btn_fechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_imprimir_Click(object sender, EventArgs e)
        {
            string nomeArquivo = Globais.caminho + @"\turmas.pdf";
            FileStream arquivoPdf = new FileStream(nomeArquivo, FileMode.Create);
            Document doc = new Document(PageSize.A4);
            PdfWriter escritorPdf = PdfWriter.GetInstance(doc, arquivoPdf);



            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Globais.caminho + @"\logo.png");
            logo.ScaleToFit(100f, 100f);
            //logo.Alignment = Element.ALIGN_LEFT;
            logo.SetAbsolutePosition(10f, 740f);

            string dados = "";

            Paragraph paragrafo1 = new Paragraph(dados, new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 20, (int)System.Drawing.FontStyle.Bold));
            paragrafo1.Alignment = Element.ALIGN_CENTER;
            paragrafo1.Add("Lucas Essentials\n");
            paragrafo1.Font = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14, (int)System.Drawing.FontStyle.Italic);
            paragrafo1.Add("Programa em C#\n");
            string texto = "intagram.com/_._lucascosta_._\n\n";
            paragrafo1.Font = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 12, (int)System.Drawing.FontStyle.Regular);
            paragrafo1.Add(texto+"\n");

            Paragraph paragrafo2 = new Paragraph(dados, new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 20, (int)System.Drawing.FontStyle.Bold));
            paragrafo2.Alignment = Element.ALIGN_LEFT;
            texto = "Aqui vem o texto do 2° parágrafo\n\n\n\n\n";
            paragrafo2.Font = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 12, (int)System.Drawing.FontStyle.Regular);
            paragrafo2.Add(texto);

            //
            // Criação da tabela.
            //
            PdfPTable tabela = new PdfPTable(3);
            tabela.DefaultCell.FixedHeight = 20;

            PdfPCell celula1 = new PdfPCell();
            celula1.AddElement(logo);
            celula1.Colspan = 3;
            //celula.Rotation = 90;
            tabela.AddCell(celula1);

            tabela.AddCell("ID");
            tabela.AddCell("Turma");
            tabela.AddCell("Horário");

            //
            // Inserir dados do banco no pdf.
            //
            DataTable dtTurmas = Banco.dql(vqueryler);
            for (int i = 0; i < dtTurmas.Rows.Count; i++)
            {
                tabela.AddCell(dtTurmas.Rows[i].Field<Int64>("ID").ToString());
                tabela.AddCell(dtTurmas.Rows[i].Field<string>("Matéria"));
                tabela.AddCell(dtTurmas.Rows[i].Field<string>("Horário"));
            }

            //
            // Gerar pdf
            //
            doc.Open();
            doc.Add(logo);
            doc.Add(paragrafo1);
            doc.Add(paragrafo2);
            doc.Add(tabela);
            doc.Close();

            //var res = MessageBox.Show("Deseja abrir o relatório?", "Relatório", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (res == DialogResult.Yes)
            //{
                System.Diagnostics.Process.Start(Globais.caminho + @"\turmas.pdf");
            //}

        }
    }
}
