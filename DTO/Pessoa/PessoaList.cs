using System;

namespace RHControl.DTO.Pessoa
{
    [Serializable]
    public class PessoaList
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string Salario { get; set; }
    }
}