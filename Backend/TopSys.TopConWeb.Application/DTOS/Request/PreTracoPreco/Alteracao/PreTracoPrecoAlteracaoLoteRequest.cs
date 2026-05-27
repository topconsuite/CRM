using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Enums;

namespace TopSys.TopConWeb.Application.DTOS.Request.PreTracoPreco.Alteracao
{
    public class PreTracoPrecoAlteracaoLoteRequest
    {
        public ETipoAlteracaoLoteTabelaVenda Tipo { get; set; }
        public float Valor { get; set; }
        public PreTracoPrecoAlteracaoRequest[] Tracos { get; set; }
    }
}
