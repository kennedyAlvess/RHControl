<%@ Page Language="C#" AutoEventWireup="true" Title="Relatórios" CodeBehind="Relatorios.aspx.cs" MasterPageFile="~/Site.Master" Inherits="RHControl.Views.Relatorios" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" Visible="false" ContentPlaceHolderID="MainContent" runat="server">

    <h3 class="mb-4">Funcionários</h3>

    <div class="card text-left col-sm-6 cl-md-6 mt-5 mb-5">
        <div class="card-header">
            Salário Funcionário
        </div>
        <div class="card-body">
            <div class="d-flex flex-column">
                <label>Cargo:</label>
                <div class="d-flex flex-wrap gap-2 mb-3 align-items-start">
                    <asp:DropDownList runat="server" ID="ddlFiltroCargoSalarios" CssClass="form-select form-select-sm" Style="width: 150px;"></asp:DropDownList>

                    <asp:LinkButton runat="server" CommandName="Salarios" OnClick="lkExcel_Click"
                        CssClass="btn btn-success btn-sm d-flex align-items-center justify-content-center"
                        Style="min-width: 120px;">
                <i class="bi bi-file-earmark-excel me-2"></i> Planilha
                    </asp:LinkButton>

                    <asp:LinkButton runat="server" OnClick="lkPdf_Click"
                        CssClass="btn btn-danger btn-sm d-flex align-items-center justify-content-center"
                        Style="min-width: 120px;">
                <i class="bi bi-file-earmark-pdf me-2"></i> PDF
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="card-footer text-muted">
            Relatório contendo dados dos salários dos funcionários.
        </div>
    </div>

    <div class="card text-left col-sm-6 cl-md-6 mt-5 mb-5">
        <div class="card-header">
            Funcionários
        </div>
        <div class="card-body d-flex">
            <div class="d-flex flex-column">
                <label>Cargo:</label>
                <div class="d-flex flex-wrap gap-2 mb-3 align-items-start">
                    <asp:DropDownList runat="server" ID="ddlFiltroCargoFuncionarios" CssClass="form-select form-select-sm" Style="width: 150px;"></asp:DropDownList>

                    <asp:LinkButton runat="server" CommandName="Funcionarios" OnClick="lkExcel_Click"
                        CssClass="btn btn-success btn-sm d-flex align-items-center justify-content-center"
                        Style="min-width: 120px;">
                <i class="bi bi-file-earmark-excel me-2"></i> Planilha
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="card-footer text-muted">
            Relatório dos funcionários cadastrados no sistema.
        </div>
    </div>
</asp:Content>
