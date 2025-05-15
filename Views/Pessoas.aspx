<%@ Page Language="C#" Title="Funcionários" AutoEventWireup="true" CodeBehind="Pessoas.aspx.cs" MasterPageFile="~/Site.Master" Inherits="RHControl.Views.Pessoas"  Async="true"%>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.mask/1.14.16/jquery.mask.min.js"></script>
    <script src="<%= ResolveUrl("~/Scripts/mascaras.js") %>"></script>
    <script>
        function UpdatePanel() {
            Mascaras();
        }
    </script>
</asp:Content>

<asp:Content ID="BodyContent" Visible="false" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">Sys.WebForms.PageRequestManager.getInstance().add_endRequest(UpdatePanel);</script>

    <h3 class="mb-4">Funcionários</h3>
    <div class="d-flex flex-wrap justify-content-between align-items-end mb-4 gap-3">
        <div class="d-flex flex-column flex-sm-row align-items-sm-end gap-2">
            <div class="form-floating">
                <asp:DropDownList runat="server" ID="ddlFiltroCargoFuncionarios" CssClass="form-select form-select-sm" Style="width: 150px;"></asp:DropDownList>
                <label for="ddlFiltroCargoFuncionarios">Cargo</label>
            </div>
            <div class="form-floating">
                <asp:HiddenField runat="server" ID="hfPesquisar" />
                <asp:TextBox runat="server" ID="txtPesquisar" CssClass="form-control" placeholder="Nome" />
                <label for="txtPesquisar">Nome</label>
            </div>
            <asp:Button runat="server" ID="btnPesquisar" OnClick="btnPesquisar_Click" Text="Pesquisar" CssClass="btn btn-secondary" />
        </div>
        <div class="d-flex gap-2">
            <asp:Button runat="server" ID="btnCalcularSalarios" Text="Calcular Salários" OnClientClick="showLoading()" OnClick="btnCalcularSalarios_Click" CssClass="btn btn-secondary" />
            <asp:Button runat="server" OnClick="btnAdicionar_Click" class="btn btn-success" Text="+ Adicionar" />
        </div>
    </div>
    <asp:Label ID="msgSemRegistros" runat="server" class="text-danger d-block mt-3" Visible="false">Nenhum registro encontrado.
    </asp:Label>


    <div class="modal fade" id="pessoaModal" tabindex="-1" aria-labelledby="pessoaModalLabel">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 runat="server" class="modal-title" id="modalTitle">Cadastro/Editar Usuário</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                </div>
                <asp:UpdatePanel ID="updatePanelModal" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hfPessoaId" runat="server" />
                        <div class="modal-body">
                            <div id="msgErro" runat="server" visible="false" class="alert alert-danger" role="alert">
                                Preencha todos os campos para continuar.
                            </div>
                            <div class="row g-3">
                                <div class="col-md-4 form-floating">
                                    <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" placeholder="Nome" />
                                    <label for="txtDataNascimento">&nbsp; Nome</label>
                                </div>
                                <div class="col-md-4 form-floating">
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Usuário" />
                                    <label for="txtDataNascimento">&nbsp; Usuário</label>
                                </div>
                                <div class="col-md-4 form-floating">
                                    <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <label for="ddlCargo">&nbsp; Cargo</label>
                                </div>

                                <div class="col-md-4 form-floating">
                                    <asp:TextBox ID="txtDataNascimento" runat="server" CssClass="form-control" TextMode="Date" />
                                    <label for="txtDataNascimento">&nbsp; Data de Nascimento</label>
                                </div>
                                <div class="col-md-4 form-floating">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="E-mail" TextMode="Email" />
                                    <label for="txtDataNascimento">&nbsp; E-mail</label>
                                </div>
                                <div class="col-md-4 form-floating">
                                    <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control telefone" placeholder="Telefone" />
                                    <label for="txtDataNascimento">&nbsp; Telefone</label>
                                </div>

                                <div class="col-md-3 form-floating">
                                    <asp:TextBox ID="txtCep" runat="server" CssClass="form-control cep" OnTextChanged="txtCep_TextChanged" placeholder="CEP" AutoPostBack="true" />
                                    <label for="txtDataNascimento">&nbsp; CEP</label>
                                </div>
                                <div class="col-md-3 form-floating">
                                    <asp:TextBox ID="txtCidade" runat="server" CssClass="form-control" placeholder="Cidade" />
                                    <label for="txtDataNascimento">&nbsp; Cidade</label>
                                </div>
                                <div class="col-md-3 form-floating">
                                    <asp:TextBox ID="txtEndereco" runat="server" CssClass="form-control" placeholder="Endereço" />
                                    <label for="txtDataNascimento">&nbsp; Endereço</label>
                                </div>
                                <div class="col-md-3 form-floating">
                                    <asp:TextBox ID="txtPais" runat="server" CssClass="form-control" placeholder="País" />
                                    <label for="txtDataNascimento">&nbsp; País</label>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <asp:LinkButton runat="server" ID="btnSalvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" Text="Salvar" />
                                <asp:LinkButton runat="server" ID="btnCancelar" OnClick="btnCancelar_Click" CssClass="btn btn-secondary" Text="Cancelar" />
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSalvar" />
                        <asp:PostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <asp:GridView
        ShowHeaderWhenEmpty="true"
        ID="gridView"
        runat="server"
        AutoGenerateColumns="False"
        AllowPaging="true"
        OnPageIndexChanging="GridView_PageIndexChanging"
        OnRowCommand="GridView1_RowCommand"
        CssClass="table table-bordered table-striped align-middle"
        PagerStyle-CssClass="pagination-data"
        PagerStyle-PageButtonCssClass="pagination-link"
        PageSize="8">

        <PagerSettings Mode="NumericFirstLast"
            NextPageText=">"
            PreviousPageText="<"
            FirstPageText="«"
            LastPageText="»"
            Position="Bottom" />
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome">
                <ItemStyle Width="250px" />
            </asp:BoundField>

            <asp:BoundField DataField="Cargo" HeaderText="Cargo">
                <ItemStyle Width="150px" />
            </asp:BoundField>

            <asp:BoundField DataField="Salario" HeaderText="Salario">
                <ItemStyle Width="120px" />
            </asp:BoundField>


            <asp:TemplateField HeaderText="Ações" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <div class="d-flex justify-content-center gap-2">
                        <asp:LinkButton
                            ID="lkCalcular"
                            runat="server"
                            CommandName="Calcular"
                            CommandArgument='<%# Eval("ID") %>'
                            CssClass="btn btn-primary btn-sm"
                            ToolTip="Calcular Salário">
                            <i class="bi bi-calculator-fill"></i>
                        </asp:LinkButton>

                        <asp:LinkButton
                            ID="lkEditar"
                            runat="server"
                            CommandName="Editar"
                            CommandArgument='<%# Eval("ID") %>'
                            CssClass="btn btn-warning btn-sm"
                            ToolTip="Editar">
                            <i class="bi bi-pencil"></i>
                        </asp:LinkButton>

                        <asp:LinkButton
                            ID="lkExcluir"
                            runat="server"
                            CommandName="Excluir"
                            CommandArgument='<%# Eval("ID") %>'
                            OnClientClick="return confirmarExclusao(this);"
                            CssClass="btn btn-danger btn-sm"
                            ToolTip="Excluir">
                            <i class="bi bi-trash"></i>
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
