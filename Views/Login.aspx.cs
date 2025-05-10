using RHControl.Repository;
using System;
using System.Web.Security;
using System.Web.UI;

namespace RHControl.Views
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            { return; }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            UsuarioRepository repository = new UsuarioRepository();
            string login = txtUsuario.Text;
            string senha = txtSenha.Text;
            if (repository.AutenticarUsuario(login, senha))
            {
                FormsAuthentication.RedirectFromLoginPage(login, false);
            }
            else
            {
                lblMensagemErro.Text = "Usuário ou senha inválidos.";
                lblMensagemErro.Visible = true;
            }
        }
    }
}