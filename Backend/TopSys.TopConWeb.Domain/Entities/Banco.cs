namespace TopSys.TopConWeb.Domain.Entities
{
    public class Banco
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string RazaoSocial { get; set; }

        public int CodigoOficial { get; set; }

        public int NumeroAgencia { get; set; }

        public string DigitoVerificadorAgencia { get; set; }

        public int NumeroConta { get; set; }

        public string DigitoVerificadorConta { get; set; }

        public int Empresa { get; set; }

        public int Empresa_proprietaria { get; set; }
    }
}
