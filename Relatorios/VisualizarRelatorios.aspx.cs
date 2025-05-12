using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using RHControl.Repository;
using System;
using System.Web.UI;

namespace RHControl.Relatorios
{
    public partial class VisualizarRelatorioPdf : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                CrystalReportViewer.ReportSource = Session["Relatorio"];
                return;
            }

            RedirecionarRelatorio();
        }

        private void RedirecionarRelatorio()
        {
            string tipoRelatorio = Request.QueryString["tipoRelatorio"];
            string rel = Request.QueryString["rel"];

            if (string.IsNullOrWhiteSpace(tipoRelatorio))
            {
                return;
            }

            int cargoId = Request.QueryString["cargoId"] != null ? Convert.ToInt32(Request.QueryString["cargoId"]) : 0;
            if (tipoRelatorio.Equals("Excel", StringComparison.OrdinalIgnoreCase))
            {
                GerarRelatorioExcel(cargoId, rel);
            }
            else if (tipoRelatorio.Equals("Pdf", StringComparison.OrdinalIgnoreCase))
            {
                GerarRelatorioPdf(cargoId);
            }

        }

        private void GerarRelatorioExcel(int cargoId, string rel)
        {
            ReportDocument novoRelatorio = new ReportDocument();

            if(rel == "Salarios")
            {
                novoRelatorio.Load(Server.MapPath("~/Relatorios/FuncionariosSalarios.rpt"));
            }
            else
            {
                novoRelatorio.Load(Server.MapPath("~/Relatorios/FuncionariosDados.rpt"));
            }
            novoRelatorio.SetParameterValue("cargoId", cargoId);

            CrystalReportRepository.SetReportConnection(novoRelatorio);

            CrystalReportViewer.ReportSource = novoRelatorio;
            Session["Relatorio"] = novoRelatorio;

            novoRelatorio.ExportToHttpResponse(ExportFormatType.Excel, Response, true, rel);
        }

        private void GerarRelatorioPdf(int cargoId)
        {
            ReportDocument novoRelatorio = new ReportDocument();
            novoRelatorio.Load(Server.MapPath("~/Relatorios/FuncionariosSalarios.rpt"));
            novoRelatorio.SetParameterValue("cargoId", cargoId);
            CrystalReportRepository.SetReportConnection(novoRelatorio);

            CrystalReportViewer.ReportSource = novoRelatorio;
            Session["Relatorio"] = novoRelatorio;

            novoRelatorio.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Salarios");
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (Session["Relatorio"] is ReportDocument Documento)
            {
                Documento.Close();
                Documento.Dispose();
            }
        }
    }
}