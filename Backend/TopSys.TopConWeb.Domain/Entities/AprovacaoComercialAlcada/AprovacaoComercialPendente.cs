using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada
{
    using System;
    using TopSys.TopConWeb.Domain.Enums;

    public class AprovacaoComercialPendente
    {
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }

        public int ObraVersao { get; set; }
        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }

        public int NivelHierarquia { get; set; }
        public EAprovacaoComercialPendenteStatus AprovacaoStatus { get; set; }

        public DateTime? AprovacaoData { get; set; }

        public virtual ICollection<AprovacaoComercialPendenteTraco> Tracos { get; set; }
        public virtual ICollection<AprovacaoComercialPendenteBomba> Bombas { get; set; }
        public virtual ICollection<AprovacaoComercialPendenteVolume> Volumes { get; set; }
        public virtual ICollection<AprovacaoComercialPendenteCondicaoPagamento> CondicoesPagamento { get; set; }


    }

    public class AprovacaoComercialPendenteTraco
    {
        public Guid Id { get; set; }
        public Guid IdAprovacao { get; set; }

        public int ObraVersao { get; set; }
        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }
        public int ObraSeq { get; set; }

        public int NivelHierarquia { get; set; }
        public EAprovacaoComercialPendenteStatus AprovacaoStatus { get; set; }

        public DateTime? AprovacaoData { get; set; }
        public string AprovacaoUsuario { get; set; } = "";
        public int AprovacaoSequencia { get; set; }

        public bool PendenteAprovacaoComercial() => AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao || AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;

    }

    public class AprovacaoComercialPendenteBomba
    {
        public Guid Id { get; set; }
        public Guid IdAprovacao { get; set; }

        public int ObraVersao { get; set; }
        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }
        public int ObraSeq { get; set; }

        public int NivelHierarquia { get; set; }
        public EAprovacaoComercialPendenteStatus AprovacaoStatus { get; set; }

        public DateTime? AprovacaoData { get; set; }
        public string AprovacaoUsuario { get; set; } = "";
        public int AprovacaoSequencia { get; set; }

        public bool PendenteAprovacaoComercial() => AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao || AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;
    }

    public class AprovacaoComercialPendenteVolume
    {
        public Guid Id { get; set; }
        public Guid IdAprovacao { get; set; }

        public int ObraVersao { get; set; }
        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }

        public int NivelHierarquia { get; set; }
        public EAprovacaoComercialPendenteStatus AprovacaoStatus { get; set; }

        public DateTime? AprovacaoData { get; set; }
        public string AprovacaoUsuario { get; set; } = "";
        public int AprovacaoSequencia { get; set; }

        public bool PendenteAprovacaoComercial() => AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao || AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;
    }

    public class AprovacaoComercialPendenteCondicaoPagamento
    {
        public Guid Id { get; set; }
        public Guid IdAprovacao { get; set; }

        public int ObraVersao { get; set; }
        public int ObraUsina { get; set; }
        public int ObraNumero { get; set; }

        public int NivelHierarquia { get; set; }
        public EAprovacaoComercialPendenteStatus AprovacaoStatus { get; set; }

        public DateTime? AprovacaoData { get; set; }
        public string AprovacaoUsuario { get; set; } = "";
        public int AprovacaoSequencia { get; set; }

        public bool PendenteAprovacaoComercial() => AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacao || AprovacaoStatus == EAprovacaoComercialPendenteStatus.AguardandoAprovacaoNivelAnterior;
    }

}
