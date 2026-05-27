using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Conta
    {
        public int EmpresaCodigo { get; set; }

        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Razao { get; set; }

        public int BancoCodigoOficial { get; set; }

        public int NumeroAgencia { get; set; }

        public long NumeroConta { get; set; }

        public string DvAgencia { get; set; }

        public string DvConta { get; set; }

        public int EmpresaProprietaria { get; set; }

        public DateTime? DataUltimaConciliacao { get; set; }

        public bool IsConciliadaNaData(DateTime? data)
        {
            return DataUltimaConciliacao >= data;
        }
    }
}
