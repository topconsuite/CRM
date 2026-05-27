using System;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class NotaFiscalFisicaItem
    {
        public int Cfop { get; set; }
        public double CustoTotal { get; set; }
        public DateTime? DataHoraUltimaAtualizacao { get; set; }
        public DateTime? DataOperacao { get; set; }
        public int FilialCodigo { get; set; }
        public string IdAtual { get; set; }
        public string IdCadastro { get; set; }
        public int IntervenienteEstoque { get; set; }
        public int IntervenienteCodigo { get; set; }
        public string LocalEstoque { get; set; }
        public int LocalInsumo { get; set; }
        public string MercadoriaCodigo { get; set; }
        public virtual Mercadoria Mercadoria { get; set; }
        public string IdExternoMercadoria => Mercadoria?.IdExterno;
        public long Numero { get; set; }
        public int SequenciaItem { get; set; }
        public double PercentualAjuste { get; set; }
        public double Peso { get; set; }
        public double PrecoUnitario { get; set; }
        public double Quantidade { get; set; }
        public double QuantidadeComissao { get; set; }
        public double QuantidadeEstoque { get; set; }
        public double QuantidadeTeorica { get; set; }
        public int SequenciaCfop { get; set; }
        public int Sequencia { get; set; }
        public string Serie { get; set; }
        public int TipoDocumentoCodigo { get; set; }
        public int TipoEstoque { get; set; }
        public string TracoConcreto { get; set; }
        public int Transacao { get; set; }
        public float? Umidade { get; set; }
        public double ValorDesconto { get; set; }
        public double ValorFrete { get; set; }
        public double ValorOutrasDespesas { get; set; }
        public double ValorSeguro { get; set; }
        public double ValorTotal { get; set; }
        public double Volume { get; set; }
    }
}
