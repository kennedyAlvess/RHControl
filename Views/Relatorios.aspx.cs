using RHControl.Repository;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RHControl.Views
{
    public partial class Relatorios : Page
    {
        readonly CargoRepository _cargoRepository = new CargoRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            CarregarDropDownsLists();
        }
        protected void lkExcel_Click(object sender, EventArgs e)
        {
            LinkButton rel = (LinkButton)sender;

            int cargoId = rel.CommandName == "Salarios" ? Convert.ToInt32(ddlFiltroCargoSalarios.SelectedItem.Value) : Convert.ToInt32(ddlFiltroCargoFuncionarios.SelectedItem.Value);
            string script = $"window.open('{ResolveUrl(string.Format("~/Relatorios/VisualizarRelatorios.aspx?cargoId={0}&tipoRelatorio={2}&rel={1}", cargoId, rel.CommandName, "Excel"))}', '_blank');";
            ClientScript.RegisterStartupScript(this.GetType(), "AbrirRelatorio", script, true);
        }
        protected void lkPdf_Click(object sender, EventArgs e)
        {
            int cargoId =  Convert.ToInt32(ddlFiltroCargoSalarios.SelectedItem.Value);
            string script = $"window.open('{ResolveUrl(string.Format("~/Relatorios/VisualizarRelatorios.aspx?cargoId={0}&tipoRelatorio={2}&rel={1}", cargoId, "pdf", "Pdf"))}', '_blank');";
            ClientScript.RegisterStartupScript(this.GetType(), "AbrirRelatorio", script, true);
        }
        protected void CarregarDropDownsLists()
        {
            var cargos = _cargoRepository.GetCargos();

            ddlFiltroCargoFuncionarios.DataSource = cargos;
            ddlFiltroCargoFuncionarios.DataTextField = "Nome";
            ddlFiltroCargoFuncionarios.DataValueField = "Id";
            ddlFiltroCargoFuncionarios.DataBind();
            ddlFiltroCargoFuncionarios.Items.Insert(0, new ListItem("Todos", "0"));

            ddlFiltroCargoSalarios.DataSource = cargos;
            ddlFiltroCargoSalarios.DataTextField = "Nome";
            ddlFiltroCargoSalarios.DataValueField = "Id";
            ddlFiltroCargoSalarios.DataBind();
            ddlFiltroCargoSalarios.Items.Insert(0, new ListItem("Todos", "0"));
        }
    }
}