using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RHControl.DTO.PessoaSalario
{
    public class PessoaSalarioModel
    {
        public string Nome { get; set; }
        public string SalarioBruto { get; set; }
        public string Desconto { get; set; }
        public string SalarioLiquido { get; set; }
    }
}