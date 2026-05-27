using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.CadastroDiverso;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICadastroDiversoApplicationService : IApplicationServiceBase<CadastroDiverso>
    {
        ICollection<CadastroDiversoResponse> ListarAndares();

        ICollection<CadastroDiversoResponse> ListarCondicoesPagamento();

        ICollection<CadastroDiversoResponse> ListarDiasDaSemanaFixo();

        ICollection<CadastroDiversoResponse> ListarOpcoesDeVencimentoEmDiaNaoUtil();

        ICollection<CadastroDiversoResponse> ListarQuantidadeDeCorposDeProva();

        ICollection<CadastroDiversoResponse> ListarModeloDocumentoRemessaConcreto();
    }
}
