<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="VisualizarRelatorios.aspx.cs" Inherits="RHControl.Relatorios.VisualizarRelatorioPdf" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="cr" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" Visible="false" ContentPlaceHolderID="MainContent" runat="server">
    <cr:CrystalReportViewer ID="CrystalReportViewer" runat="server"
        AutoDataBind="true"
        EnableDatabaseLogonPrompt="false"
        EnableParameterPrompt="false"
        ToolPanelView="None"
        Width="100%" Height="900px" />
</asp:Content>
