using System;

namespace TopSys.TopConWeb.Domain.Interfaces.Entities
{
    public interface IObraPagamentoDetalhe
    {
        int UsinaCodigo { get; set; }

        int PropostaAno { get; set; }

        int PropostaNumero { get; set; }

        int ObraCodigo { get; set; }

        int ContratoAno { get; set; }

        int ContratoNumero { get; set; }

        int PagamentoSequencia { get; set; }

        int DetalheSequencia { get; set; }

        float Valor { get; set; }

        string IdCadastro { get; set; }

        string IdAtualizacao { get; set; }

        string InfoString();
        DateTime? DataTitulo();
    }
}
