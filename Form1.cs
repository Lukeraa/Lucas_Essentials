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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            F_Login f_Login = new F_Login(this);
            f_Login.ShowDialog();
        }

        //
        // Para chamar formulário de login
        //
        private void lOGARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deslogar();
            DeslogadoMenu();
            F_Login f_Login = new F_Login(this);
            f_Login.ShowDialog();
        }

        //
        // Para deslogar
        //
        public void Deslogar()
        {
            if (Globais.logado == false)
            {
                DeslogadoMenu();
            }
            else if(Globais.logado == true)
            {
                lb_acesso.Text = "0";
                lb_nomeuser.Text = "---";
                pb_ledlogado.Image = Properties.Resources.vermelho;
                Globais.nvl = 0;
                Globais.logado = false;
                DeslogadoMenu();
            }
        }

        private void dESLOGARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deslogar();
            Close();
        }

        //
        // Bloquear o menu para quem não estiver logado.
        //
        private void DeslogadoMenu()
        {
            if (Globais.logado == false)
            {
                alunosToolStripMenuItem.Visible = false;
                usuáriosToolStripMenuItem.Visible = false;
                manutençãoToolStripMenuItem.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            
            this.Text = "Escola Lucas Costa - " + fvi.FileVersion;
            DeslogadoMenu();
        }

        private void novoUsuárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_NovoUsuario f_NovoUsuario = new F_NovoUsuario();
            f_NovoUsuario.ShowDialog();
        }

        private void gestãoDeUsuáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_GestaoUsuarios f_GestaoUsuarios = new F_GestaoUsuarios();
            f_GestaoUsuarios.ShowDialog();
        }

        private void horáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Horarios f_Horarios = new F_Horarios();
            f_Horarios.ShowDialog();
        }

        private void professoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_GestaoProfessores f_GestaoProfessores = new F_GestaoProfessores();
            f_GestaoProfessores.ShowDialog();
        }

        private void turmasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_Turmas f_Turmas = new F_Turmas();
            f_Turmas.ShowDialog();
        }

        private void novoAlunoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_NovoAluno f_NovoAluno = new F_NovoAluno();
            f_NovoAluno.ShowDialog();
        }

        private void gestãoDeAlunosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F_GestaoAlunos f_GestaoAlunos = new F_GestaoAlunos();
            f_GestaoAlunos.ShowDialog();
        }
    }
}
