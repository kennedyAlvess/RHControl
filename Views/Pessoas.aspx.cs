using RHControl.DTO.Pessoa;
using RHControl.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RHControl.Views
{
    public partial class Pessoas : Page
    {
        readonly PessoaRepository _pessoaRepository = new PessoaRepository();
        readonly CargoRepository _cargoRepository = new CargoRepository();
        readonly PessoaSalarioRepository _pessoaSalarioRepository = new PessoaSalarioRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            BindGridViewData(new List<PessoaList>());
            CarregarCargos();
        }

        #region Eventos
        protected void txtCep_TextChanged(object sender, EventArgs e)
        {
            string cep = txtCep.Text.Replace("-", "");

            if (!(cep.Length == 8) || !int.TryParse(cep, out _))
            {
                ExibirAlerta("Aviso!", "Informe um CEP valido.", "error");
                return;
            }
            ValidarCep(cep);
        }
        protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (gridView != null)
            {
                gridView.PageIndex = e.NewPageIndex;
                List<PessoaList> pessoas = ViewStatePessoas ?? _pessoaRepository.GetPessoas("", 0, 0);
                BindGridViewData(pessoas);
            }
        }
        #endregion

        #region Ações de Botões
        protected async void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Editar":
                    {
                        int id = Convert.ToInt32(e.CommandArgument);
                        modalTitle.InnerText = "Editar Funcionário";
                        PessoaModel pessoa = _pessoaRepository.GetPessoaById(id);
                        PreencherCampos(pessoa);
                        ResetarCamposCss();
                        ExibirModal();
                        break;
                    }
                case "Excluir":
                    {
                        int id = Convert.ToInt32(e.CommandArgument);
                        if (_pessoaRepository.Remove(id) == 0)
                        {
                            ExibirAlerta("Aviso!", "Funcionário não localizado.", "error");
                            return;
                        }
                        ExibirAlerta("Sucesso!", "Funcionário removido com sucesso.", "success");
                        BindGridViewData(_pessoaRepository.GetPessoas("",0,0));
                        break;
                    }
                case "Calcular":
                    {
                        int id = Convert.ToInt32(e.CommandArgument);
                        bool isCalculado = await _pessoaRepository.CalcularSalarioPessoa(id);

                        if (!isCalculado)
                        {
                            ExibirAlerta("Aviso!", "Funcionário não localizado.", "error");
                            break;
                        }

                        ExibirAlerta("Sucesso!", "Salário calculado com sucesso.", "success");

                        BindGridViewData(_pessoaRepository.GetPessoas("",0,0));
                        break;
                    }
                default:
                    break;

            }
        }
        protected async void btnCalcularSalarios_Click(object sender, EventArgs e)
        {
            try
            {
                await _pessoaRepository.CalcularSalarios();
                BindGridViewData(_pessoaRepository.GetPessoas("", 0, 0));
                ExibirAlerta("Sucesso!", "Salários calculados com sucesso.", "success");
            }
            catch (Exception ex)
            {
                ExibirAlerta("Aviso!", ex.Message, "error");
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoading", "hideLoading();", true);
            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            int cargoId = Convert.ToInt32(ddlFiltroCargoFuncionarios.SelectedItem.Value);
            BindGridViewData(_pessoaRepository.GetPessoas(txtPesquisar.Text.ToUpper(), cargoId, cargoId));
        }
        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            modalTitle.InnerText = "Adicionar Pessoa";
            ResetarCamposCss();
            PreencherCampos(new PessoaModel());
            ExibirModal();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        protected async void btnSalvar_Click(object sender, EventArgs e)
        {
            bool isInsert = string.IsNullOrWhiteSpace(hfPessoaId.Value);

            if (!ValidarCampos())
            {
                ExibirModal();
                return;
            }

            var pessoa = new PessoaModel
            {
                Nome = txtNome.Text,
                Cidade = txtCidade.Text,
                Email = txtEmail.Text,
                CEP = txtCep.Text,
                Endereco = txtEndereco.Text,
                Pais = txtPais.Text,
                Usuario = txtUsuario.Text,
                Telefone = txtTelefone.Text,
                Data_Nascimento = txtDataNascimento.Text,
                Cargo_Id = int.Parse(ddlCargo.SelectedItem.Value)
            };

            if(!isInsert)
                pessoa.Id = Convert.ToInt32(hfPessoaId.Value);

            int rowsAffected = _pessoaRepository.InsertOrUpdate(pessoa);
            if (rowsAffected == 0)
            {
                ExibirAlerta("Aviso!", "Erro ao realizar a ação!", "error");
                return;
            }

            if(!isInsert && _pessoaSalarioRepository.PessoaSalarioExiste((int)pessoa.Id))
                await _pessoaRepository.CalcularSalarioPessoa(pessoa.Id.Value);

            ExibirAlerta("Concluido", "", "success");
            BindGridViewData(_pessoaRepository.GetPessoas("", 0, 0));
        }
        #endregion

        #region Metodos
        protected void ExibirAlerta(string titulo, string mensagem, string tipo)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", $"showAlert('{titulo}', '{mensagem}', '{tipo}');", true);
        }
        private void PreencherCampos(PessoaModel pessoa)
        {
            hfPessoaId.Value = pessoa.Id.ToString();
            txtNome.Text = pessoa.Nome;
            txtUsuario.Text = pessoa.Usuario;
            txtCidade.Text = pessoa.Cidade;
            txtCep.Text = pessoa.CEP;
            txtEndereco.Text = pessoa.Endereco;
            txtPais.Text = pessoa.Pais;
            txtEmail.Text = pessoa.Email;
            txtTelefone.Text = pessoa.Telefone;
            txtDataNascimento.Text = pessoa.Data_Nascimento != null ? DateTime.Parse(pessoa.Data_Nascimento).ToString("yyyy-MM-dd") : "";

            ddlCargo.ClearSelection();
            ListItem item = ddlCargo.Items.FindByValue(pessoa.Cargo_Id.ToString());
            if(item != null)
                item.Selected = true;
        }
        private bool ValidarCampos()
        {
            bool isValid = true;
            ResetarCamposCss();
            var campos = new Dictionary<WebControl, bool>
            {
                { txtNome, !string.IsNullOrWhiteSpace(txtNome.Text)},
                { txtUsuario, !string.IsNullOrWhiteSpace(txtUsuario.Text) },
                { txtCidade, !string.IsNullOrWhiteSpace(txtCidade.Text)},
                { txtCep, !string.IsNullOrWhiteSpace(txtCep.Text) },
                { txtEndereco, !string.IsNullOrWhiteSpace(txtEndereco.Text) },
                { txtPais, !string.IsNullOrWhiteSpace(txtPais.Text) },
                { txtEmail, !string.IsNullOrWhiteSpace(txtEmail.Text) },
                { txtTelefone, !string.IsNullOrWhiteSpace(txtTelefone.Text) },
                { txtDataNascimento, !string.IsNullOrWhiteSpace(txtDataNascimento.Text) },
                { ddlCargo, !(ddlCargo.SelectedItem.Value == "0")}
            };

            foreach (var campo in campos)
            {
                if (!campo.Value && !campo.Key.CssClass.Contains("is-invalid"))
                {
                    campo.Key.CssClass += " is-invalid";
                    isValid = false;
                }
                else if(campo.Value)
                {
                    campo.Key.CssClass = campo.Key.CssClass.Replace(" is-invalid", "");
                }
            }

            msgErro.Visible = !isValid;
            return isValid;
        }
        private void ResetarCamposCss()
        {
            msgErro.Visible = false;
            var campos = new List<WebControl>
            {
                txtNome, 
                txtUsuario, 
                txtCidade,
                txtCep, 
                txtEndereco, 
                txtPais, 
                txtEmail, 
                txtTelefone, 
                txtDataNascimento,
                ddlCargo
            };

            foreach (var campo in campos)
            {
                campo.CssClass = campo.CssClass.Replace(" is-invalid", "");
            }
        }
        private void ValidarCep(string cep)
        {
            string url = $"https://viacep.com.br/ws/{cep}/json/";

            using (WebClient wc = new WebClient())
            {
                try
                {
                    string json = wc.DownloadString(url);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    ViaCepResult result = serializer.Deserialize<ViaCepResult>(json);
                    if (result.Erro)
                    {
                        throw new ArgumentException("Cep não encontrato!");
                    }

                    txtPais.Text = "Brasil";
                    txtCidade.Text = result.Localidade;

                    if (!string.IsNullOrWhiteSpace(result.Logradouro) || !string.IsNullOrWhiteSpace(result.Bairro))
                        txtEndereco.Text = result.Logradouro+ " " +result.Bairro;
                }
                catch (Exception ex)
                {
                    ExibirAlerta("Aviso!",ex.Message,"error");
                }
            }
        }
        private DataTable PreencherDtPessoas(List<PessoaList> pessoas)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("Nome", typeof(string));
            dataTable.Columns.Add("Cargo", typeof(string));
            dataTable.Columns.Add("Salario", typeof(string));

            foreach (PessoaList pessoa in pessoas)
            {
                DataRow row = dataTable.NewRow();
                row["Id"] = pessoa.Id;
                row["Nome"] = pessoa.Nome;
                row["Cargo"] = pessoa.Cargo;
                row["Salario"] = pessoa.Salario == 0? "Salário não calculado": pessoa.Salario.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        private void BindGridViewData(List<PessoaList> pessoas)
        {
            ViewStatePessoas = pessoas;
            var dtPessoas = PreencherDtPessoas(pessoas);
            gridView.DataSource = dtPessoas;
            gridView.DataBind();
        }
        private void CarregarCargos()
        {
            var cargos = _cargoRepository.GetCargos();

            ddlCargo.DataSource = cargos;
            ddlCargo.DataTextField = "Nome";
            ddlCargo.DataValueField = "Id";
            ddlCargo.DataBind();
            ddlCargo.Items.Insert(0, new ListItem("Selecione", "0"));

            ddlFiltroCargoFuncionarios.DataSource = _cargoRepository.GetCargos();
            ddlFiltroCargoFuncionarios.DataTextField = "Nome";
            ddlFiltroCargoFuncionarios.DataValueField = "Id";
            ddlFiltroCargoFuncionarios.DataBind();
            ddlFiltroCargoFuncionarios.Items.Insert(0, new ListItem("Todos", "0"));
        }
        private void ExibirModal()
        {
            string script = "var myModal = new bootstrap.Modal(document.getElementById('pessoaModal'), {backdrop: 'static'}); myModal.show();";
            ClientScript.RegisterStartupScript(this.GetType(), "openModal", script, true);
        }
        private void FecharModal()
        {
            string script = "var myModal = new bootstrap.Modal(document.getElementById('pessoaModal')); myModal.hide();";
            ClientScript.RegisterStartupScript(this.GetType(), "closeModal", script, true);
        }
        private List<PessoaList> ViewStatePessoas
        {
            get
            {
                return ViewState["ViewStatePessoas"] as List<PessoaList> ?? new List<PessoaList>();
            }
            set
            {
                ViewState["ViewStatePessoas"] = value;
            }
        }
        #endregion
    }
}