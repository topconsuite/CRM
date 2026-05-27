using System;
using TopSys.TopConWeb.Application.DTOS.Response.TracoPreco;
using TopSys.TopConWeb.Application.DTOS.Response.ContratoReajuste;
using System.Collections;
using System.Collections.Generic;

namespace TopSys.TopConWeb.Application.DTOS.Response.ContratoTracoReajuste.ContratoTracoReajusteVigenciasResponse
{
    public class ContratoReajusteResponse
    {
        public DateTime DataVigencia { get; set; }
        public UsinaDTO UsinaEntrega { get; set; }
        public int UsinaEntregaCodigo { get; set; }
        public int UsinaCodigo { get; set; }
        public int ContratoAno { get; set; }
        public int ContratoNumero { get; set; }
        public ContratoDTO Contrato { get; set; }
        public string IdAprovacaoVersao { get; set; }
        public string IdReprovacao { get; set; }
        public DateTime? DataCarta { get; set; }
        public DateTime? DataConfirmacao { get; set; }
        public string ObraNome { get; set; }
        public IEnumerable<ObraTracoReajusteResponse> Tracos { get; set; }
        public IEnumerable<ObraBombaReajusteResponse> Bombas { get; set; }
    }
}
