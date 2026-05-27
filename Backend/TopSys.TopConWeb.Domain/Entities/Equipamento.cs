using System;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class Equipamento
    {
        public string Codigo { get; set; }
        public string ExternalID { get; set; }
        public int Tipo { get; set; }
        public string Betoneira { get; set; }
        public string Bomba { get; set; }
        public string Placa { get; set; }
        public string Descricao { get; set; }
        public int CapacidadeM3 { get; set; }
        public int MotoristaBombaUsina { get; set; }
        public int AjudanteUsina { get; set; }
        public int AjudanteCodigo { get; set; }
        public int AjudanteCodigo2 { get; set; }
        public int AjudanteCodigo3 { get; set; }
        public int UsinaAlocada { get; set; }
        public string IdCadastro { get; set; }
        public string IdAtual { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Chassis { get; set; }
        public int Ano { get; set; }
        public EEquipamentoStatus Status { get; set; }
        public DateTime? DataOcorrencia { get; set; }
        public int TipoProcesso { get; set; }
        public string Ativo { get; set; }
        public string PossuiGps { get; set; }
        public DateTime? DataInstalacaoGps { get; set; }
        public string Gps { get; set; }
        public string NumeroProcesso { get; set; }
        public string Observacao { get; set; }
        public int Grupo { get; set; }
        public int SubGrupo { get; set; }
        public int Item { get; set; }
        public int Grupo2 { get; set; }
        public int SubGrupo2 { get; set; }
        public int Item2 { get; set; }
        public int AnoModelo { get; set; }
        public string Renavam { get; set; }
        public int Combustivel { get; set; }
        public string CapacidadePotCilindros { get; set; }
        public string FaixaIpva { get; set; }
        public int Proprietario { get; set; }
        public string TacografoQuebrado { get; set; }
        public string UF { get; set; }
        public string FinalPlaca { get; set; }
        public int CapacidadeLitrosCombustivel { get; set; }
        public double PesoEquipamento { get; set; }
        public bool ControlaKm { get; set; }
        public int ControlaHorimetro { get; set; }
        public string HorimetroQuebrado { get; set; }
        public DateTime? DataSubstituicaoTacografo { get; set; }
        public DateTime? DataSubstituicaoHorimetro { get; set; }
        public int CC { get; set; }
        public int MaxKmAbastecimento { get; set; }
        public double MaxLitrosAbastecimento { get; set; }
        public int KmInicial { get; set; }
        public double HorimetroInicial { get; set; }
        public int SeguroResponsabilidade { get; set; }
        public string SeguroCnpjCpfResponsavel { get; set; }
        public string SeguroNomeSeguradora { get; set; }
        public string SeguroCnpjSeguradora { get; set; }
        public string SeguroNumeroApolice { get; set; }
        public string SeguroNumeroAverbacao { get; set; }
        public int TipoRodado { get; set; }
        public int TipoCarroceria { get; set; }
        public string RNTRC { get; set; }
        public int TipoProprietario { get; set; }
        public string FuncionarioAlocado { get; set; }
    }
}
