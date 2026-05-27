using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Domain.Entities
{
    public class DemaisAprovacao
    {

        public string Chave { get; set; }

        public int AprovacaoTipoCodigo { get; set; }

        public string UsuarioRequisitante { get; set; }

        public string UsuarioAprovacao { get; set; }

        public DateTime DataHoraSolicitacao { get; set; }

        public DateTime? DataHoraExecucao { get; set; }
        
        public string Complemento { get; set; }

        public string Observacao { get; set; }

        public virtual AprovacaoTipo AprovacaoTipo { get; set; }

        public virtual ICollection<AprovacaoScript> AprovacoesScript { get; set; }

        public EStatusAprovacao StatusAprovacao { get; private set; }

        public string LogObservacao { get; set; }

        public void AtualizaStatusAprovacao(string usuario)
        {
            if ((this.DataHoraExecucao.ToString().Equals("01/01/1000 00:00:00") || this.DataHoraExecucao == null)
                && (this.UsuarioAprovacao == "" || this.UsuarioAprovacao == usuario))
            {
                this.StatusAprovacao = EStatusAprovacao.Pendente;
            }
            else
            {
                this.StatusAprovacao = EStatusAprovacao.NaoNecessita;
            }
        }

        public void Executar(string usuario)
        {
            this.DataHoraExecucao = DateTime.Now;
            this.UsuarioAprovacao = usuario;
        }

    }
}
