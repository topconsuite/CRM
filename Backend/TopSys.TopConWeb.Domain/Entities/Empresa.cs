namespace TopSys.TopConWeb.Domain.Entities
{
    public class Empresa
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Razao { get; set; }
        public string CpfCnpj { get; set; }
        public string InscricaoEstadual { get; set; }

        public string EnderecoCep { get; set; }
        public string EnderecoLogradouro { get; set; }
        public int EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public int? EnderecoMunicipioCodigo { get; set; } = 0;
        public Municipio EnderecoMunicipio { get; set; }

        public int TelefoneDdd { get; set; }
        public int TelefoneNumero { get; set; }
        public string Email { get; set; }
        public string OptanteSimplesNacional  { get; set; }
        public float AliquotaSimplesNacional { get; set; }

        public bool SimplesNacional
        {
            get { return OptanteSimplesNacional == "S"; }
        }
    }
}
