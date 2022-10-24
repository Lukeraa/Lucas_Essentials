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
    public partial class F_NovoAluno : Form
    {
        string origemCompleto = "";
        string foto = "";
        string pastaDestino = Globais.caminhoFotos;
        string destinoCompleto = "";
        public F_NovoAluno()
        {
            InitializeComponent();
        }

        private void F_NovoAluno_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> status = new Dictionary<string, string>();
            status.Add("A", "Ativo");
            status.Add("B", "Bloqueado");
            status.Add("C", "Cancelado");
            cb_status.DataSource = new BindingSource(status, null);
            cb_status.ValueMember = "Key";
            cb_status.DisplayMember = "Value";
        }

        private void btn_novo_Click(object sender, EventArgs e)
        {
            tb_nome.Enabled = true;
            tb_nome.Clear();
            mtb_telefone.Enabled = true;
            mtb_telefone.Clear();
            cb_status.Enabled = true;
            cb_status.SelectedIndex = 0;
            tb_turma.Enabled = true;
            tb_turma.Clear();
            tb_plano.Enabled = true;
            tb_plano.Clear();
            btn_turma.Enabled = true;
            btn_plano.Enabled = true;
            btn_novo.Enabled = false;
            btn_salvar.Enabled = true;
            btn_cancelar.Enabled = true;
            btn_addFoto.Enabled = true;
            pb_foto.Image = null;
            pb_foto.BorderStyle = BorderStyle.FixedSingle;
            tb_turma.Clear();
            tb_plano.Clear();
            tb_nome.Focus();
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            tb_nome.Enabled = false;
            tb_nome.Clear();
            mtb_telefone.Enabled = false;
            mtb_telefone.Clear();
            cb_status.Enabled = false;
            cb_status.SelectedIndex = 0;
            tb_turma.Enabled = false;
            tb_turma.Clear();
            tb_plano.Enabled = false;
            tb_plano.Clear();
            btn_turma.Enabled = false;
            btn_plano.Enabled = false;
            btn_novo.Enabled = true;
            btn_salvar.Enabled = false;
            btn_cancelar.Enabled = false;
            tb_turma.Clear();
            tb_plano.Clear();
            btn_addFoto.Enabled = false;
            pb_foto.Image = null;
            pb_foto.BorderStyle = BorderStyle.None;
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            if (destinoCompleto == "")
            {
                MessageBox.Show("Para continuar selecione uma foto", "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (destinoCompleto != "")
            {
                if (File.Exists(destinoCompleto))
                {
                    if (MessageBox.Show("A foto'" + foto + "' já existe em '" + pastaDestino + "', tem certeza que deseja substituir?", "Substituir", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        btn_addFoto.Focus();
                        return;
                    }
                }
                try
                {
                    System.IO.File.Copy(origemCompleto, destinoCompleto, true);
                    if (File.Exists(destinoCompleto))
                    {
                        pb_foto.ImageLocation = destinoCompleto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Arquivo não copiado, tente novamente\n\n" + ex.Message, "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            string vquery = String.Format("INSERT INTO tb_alunos(A_NOME, A_TELEFONE, A_STATUS, A_IDTURMA, A_FOTO) VALUES('{0}', '{1}', '{2}', {3}, '{4}')", tb_nome.Text, mtb_telefone.Text, cb_status.SelectedValue, tb_turma.Tag.ToString(), destinoCompleto);
            Banco.dml(vquery);
            MessageBox.Show("Novo aluno cadastrado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            tb_nome.Enabled = false;
            tb_nome.Clear();
            mtb_telefone.Enabled = false;
            mtb_telefone.Clear();
            cb_status.Enabled = false;
            cb_status.SelectedIndex = 0;
            tb_turma.Enabled = false;
            tb_turma.Clear();
            tb_plano.Enabled = false;
            tb_plano.Clear();
            btn_turma.Enabled = false;
            btn_plano.Enabled = false;
            btn_novo.Enabled = true;
            btn_salvar.Enabled = false;
            btn_cancelar.Enabled = false;
            tb_turma.Clear();
            tb_plano.Clear();
        }

        private void btn_fechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_turma_Click(object sender, EventArgs e)
        {
            F_SelecaoTurmas f_SelecaoTurmas = new F_SelecaoTurmas(this);
            f_SelecaoTurmas.ShowDialog();
        }

        private void btn_addFoto_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                origemCompleto = openFileDialog1.FileName;
                foto = openFileDialog1.SafeFileName;
                destinoCompleto = pastaDestino + foto;
            }
            pb_foto.ImageLocation = origemCompleto;
        }
    }
}
