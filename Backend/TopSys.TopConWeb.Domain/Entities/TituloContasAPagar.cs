using System;
using Topsys.TopConWeb.SharedKernel.QueryResults;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class TituloContasAPagar : IQueryResult
    {
        public int EmpresaCodigo { get; set; }
        public int FornecedorCodigo { get; set; }
        public int DocumentoTipoCodigo { get; set; }
        public string DocumentoSerie { get; set; }
        public long DocumentoNumero { get; set; }
        public int DocumentoSequencia { get; set; }
        public int Desdobramento { get; set; }
        public int FornecedorRetidoCodigo { get; set; }
        public float Valor { get; set; }
        public long LoteBaixaDeCredito { get; set; }
        public long LoteBaixa { get; set; }
    }
}
