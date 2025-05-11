namespace RHControl.DTO.Pessoa
{
    public class PessoaModel
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Pais { get; set; }
        public string Usuario { get; set; }
        public string Telefone { get; set; }
        public string Data_Nascimento { get; set; }
        public int Cargo_Id { get; set; }
    }
}