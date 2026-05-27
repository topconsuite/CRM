using System;
using TopSys.TopConWeb.Domain.Interfaces.Entities;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ObraPagamentoDetalhe : IObraPagamentoDetalhe
    {
        public int UsinaCodigo { get; set; }

        public int PropostaAno { get; set; }

        public int PropostaNumero { get; set; }

        public int ObraCodigo { get; set; }

        public int ContratoAno { get; set; }

        public int ContratoNumero { get; set; }

        public int PagamentoSequencia { get; set; }

        public int DetalheSequencia { get; set; }

        public float Valor { get; set; }

        public string IdCadastro { get; set; }

        public string IdAtualizacao { get; set; }

        public abstract string InfoString();
        public abstract DateTime? DataTitulo();
    }
}
