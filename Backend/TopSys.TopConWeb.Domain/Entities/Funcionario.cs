namespace TopSys.TopConWeb.Domain.Entities
{
    public class Funcionario
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Ativo { get; set; }

        public string Comprador { get; set; }

        public int Usina { get; set; }

        public int Departamento { get; set; }

        public string HoraEntrada { get; set; }

        public string HoraSaida { get; set; }

        public string ExternalId { get; set; }

        public double ValorHora { get; set; }

        public int RE { get; set; }

        public int Funcao { get; set; }

        public string NomeReduzido { get; set; }

        public int Status { get; set; }

        public string UsuarioSistema { get; set; }

        public int CodigoInterveniente { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtual { get; set; }

        public virtual Interveniente Interveniente { get; set; }
    }
}
