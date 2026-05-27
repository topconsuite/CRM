using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso
{
    public class LiberacaoAcessoLog
    {
        public string TipoLiberacao { get; set; }

        public DateTime DataHoraEvento { get; set; }

        public string Usuario { get; set; }

        public string UsuarioModificado { get; set; }

        public int UsinaGrupo { get; set; }

        public string DescricaoGrupo { get; set; }

        public string Evento { get; set; }

        public string Complemento { get; set; }
    }
}
