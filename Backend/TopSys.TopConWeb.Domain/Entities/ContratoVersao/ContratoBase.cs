using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities.AssinaturaEletronicaIntegracao.Clicksign;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.SharedKernel.Helpers;

namespace TopSys.TopConWeb.Domain.Entities
{
    public abstract class ContratoBase<TContratoTracoReajuste, TContratoBombaReajuste> : IHasInterveniente, IHasVendedor, IContrato
    {
        public int Usina { get; set; }
        public int Ano { get; set; }
        public int Numero { get; set; }

        public int? IntervenienteCodigo { get; set; } = 0;

        public virtual Interveniente Interveniente { get; set; }

        public int? VendedorCodigo { get; set; } = 0;

        public virtual Vendedor Vendedor { get; set; }

        public EContratoStatus Status { get; set; }
        public EContratoStatus StatusAnterior { get; set; }

        public string FaturamentoAc { get; set; }

        public string FechadoSimNao { get; set; }
        public virtual bool Fechado
        {
            get { return (FechadoSimNao ?? "").Equals("S"); }
            set { FechadoSimNao = value ? "S" : "N"; }
        }

        public DateTime? DataEncerramento { get; set; }

        public decimal ValorConcreto { get; set; }

        public decimal ValorBomba { get; set; }

        public float ValorExtras { get; set; }

        public decimal ValorTotalContrato { get; set; }

        public float VolumeTotal { get; set; }

        public string Observacao { get; set; }

        public string CodigoObraPrefeitura { get; set; }

        public float PercentualRetencaoContratual { get; set; }

        public string MaoObraPropria { get; set; }

        public float PercentualLocacao { get; set; }

        public int ModeloDocumentoRemessaConcreto { get; set; }

        public int ModeloDocumentoRemessaBomba { get; set; }

        public int ModeloItensDanfeERomaneio { get; set; }

        public int? LocalFaturamento { get; set; } = 0;
        public int? LocalCobranca { get; set; } = 0;
        public int? ResponsavelSolidario { get; set; } = 0;

        public int? AnalistaCodigo { get; set; } = 0;

        public virtual Funcionario Analista { get; set; }

        public virtual ICollection<TContratoTracoReajuste> ContratoTracoReajustes { get; set; }

        public virtual ICollection<TContratoBombaReajuste> ContratoBombaReajustes { get; set; }

        public string IdAprovacaoVendedor { get; set; }
        public string IdAprovacaoCadastro { get; set; }
        public string CadastroAprovado { get; set; }

        public string IdAprovacaoEngenharia { get; set; }

        public string AprovaEngenharia { get; set; }

        public string Inconsistencias { get; set; }

        public void AprovarEngenharia(string usuario)
        {
            IdAprovacaoEngenharia = StringHelper.GetIDD(usuario);
        }

        public string DescricaoCoincidencia { get; set; }
        public string AguardandoAprovacao { get; set; }
        public string IdAprovacaoCoincidencia { get; set; }
        public int UsinaEntrega { get; set; }
        public bool FaturamentoPendente { get; set; }
        public string NumeroContratoAnterior { get; set; }
        public int? Segmentacao { get; set; }
        public DateTime? DataContrato { get; set; }

        public DateTime? InicioVigencia { get; set; }
        public DateTime? FimVigencia { get; set; }

        public virtual IEnumerable<ContratoClicksignEnvio> ClicksignEnvio { get; set; } = new List<ContratoClicksignEnvio>();
        public EContratoFinalidade ContratoFinalidade { get; set; }

        public EStatusClicksignDocumento StatusClicksignDocumento
        {
            get
            {
                var ultimoDocumentoEnviado = ClicksignEnvio.ToList().OrderByDescending(t => t.DataEnvio).FirstOrDefault();
                if (ultimoDocumentoEnviado != null)
                    return ultimoDocumentoEnviado.StatusClicksignDocumento;

                return EStatusClicksignDocumento.NaoEnviado;
            }
        }
        public void AprovarCoincidencia(string usuario)
        {
            AguardandoAprovacao = "N";
            IdAprovacaoCoincidencia = StringHelper.GetIDD(usuario);
        }

        public void ReprovarCoincidencia(string usuario)
        {
            AguardandoAprovacao = "R";
            IdAprovacaoCoincidencia = StringHelper.GetIDD(usuario);
        }

        public void AlterarAnalista(Funcionario analista)
        {
            AnalistaCodigo = analista?.Codigo ?? 0;
        }

        public void AprovarCadastro(string usuario)
        {
            IdAprovacaoCadastro = StringHelper.GetIDD(usuario);
            CadastroAprovado = "S";
        }

        public bool IsCadastroAprovado()
        {
            return CadastroAprovado == "S" && !string.IsNullOrEmpty(IdAprovacaoCadastro);
        }

        public void LimparAprovacaoCadastro()
        {
            IdAprovacaoCadastro = "";
            CadastroAprovado = "";
        }

        public void AlterarStatus(EContratoStatus novoStatus)
        {
            Status = novoStatus;
        }

        public void AlterarNecessitaAprovacao(string necessitaAprovacao, string idAprovacaoEngenharia)
        {
            AguardandoAprovacao = necessitaAprovacao;
            IdAprovacaoEngenharia = idAprovacaoEngenharia;
        }

        public string AprovacaoMedicao { get; set; }
        public int TempoAprovacaoMedicao { get; set; }
    }
}
